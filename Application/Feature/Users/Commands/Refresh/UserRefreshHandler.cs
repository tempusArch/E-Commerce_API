using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class UserRefreshHandler : IRequestHandler<UserRefreshCommand, string> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    public UserRefreshHandler(ECommerceApiDbContext context, IMapper mapper, IJwtService jwtService, IPasswordHasher passordHasher) {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
        _passwordHasher = passordHasher;
    }

    public async Task<string> Handle(UserRefreshCommand command, CancellationToken cancellationToken) {
        var kyuuRefreshToken = await _context.RefreshTokenTable.SingleOrDefaultAsync(n => n.Token == command.valueOfRefreshToken);

        if (kyuuRefreshToken == null || !kyuuRefreshToken.IsActive)
            throw new UnauthorizedAccessException();

        kyuuRefreshToken.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = _jwtService.Generate_RefreshToken(kyuuRefreshToken.UserId);
        _context.RefreshTokenTable.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        var um = await _context.UserTable.SingleOrDefaultAsync(n => n.Id == int.Parse(kyuuRefreshToken.UserId));
        var newAccessToken = _jwtService.Generate_JWT(um);

        return newAccessToken;
    }
}
