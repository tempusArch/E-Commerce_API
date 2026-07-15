using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;

namespace ECommerceAPI.Application;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Unit> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextService _httpContextService;
    public LogoutUserHandler(ECommerceApiDbContext context, IMapper mapper, IJwtService jwtService, IPasswordHasher passordHasher, IHttpContextService httpContextService) {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
        _passwordHasher = passordHasher;
        _httpContextService = httpContextService;
    }

    public async Task<Unit> Handle(LogoutUserCommand command, CancellationToken cancellationToken) {
        var valueOfRefreshToken = _httpContextService.GetRefreshToken();

        if (!string.IsNullOrEmpty(valueOfRefreshToken)) {
            var kyuuRefreshToken = await _context.RefreshTokenTable
                .SingleOrDefaultAsync(n => n.Token == valueOfRefreshToken);

            if (kyuuRefreshToken != null) {
                kyuuRefreshToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        _httpContextService.DeleteRefreshToken();

        return Unit.Value;
    }
}