using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class RefreshUserHandler : IRequestHandler<RefreshUserCommand, string> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextService _httpContextService;
    public RefreshUserHandler(ECommerceApiDbContext context, IMapper mapper, IJwtService jwtService, IPasswordHasher passordHasher, IHttpContextService httpContextService) {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
        _passwordHasher = passordHasher;
        _httpContextService = httpContextService;
    }

    public async Task<string> Handle(RefreshUserCommand command, CancellationToken cancellationToken) {
        var valueOfRefreshToken = _httpContextService.GetRefreshToken();

        var kyuuRefreshToken = await _context.RefreshTokenTable
            .SingleOrDefaultAsync(n => n.Token == valueOfRefreshToken);

        if (kyuuRefreshToken == null || !kyuuRefreshToken.IsActive)
            throw new UnauthorizedAccessException("RefreshToken null or invalid");

        kyuuRefreshToken.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = _jwtService.Generate_RefreshToken(kyuuRefreshToken.UserId);

        var um = await _context.UserTable.
            SingleOrDefaultAsync(
                n => n.Id == int.Parse(kyuuRefreshToken.UserId)
            );
        var newAccessToken = _jwtService.Generate_JWT(um);

        _context.RefreshTokenTable.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        _httpContextService.SetRefreshToken(newRefreshToken);

        return newAccessToken;
    }
}
