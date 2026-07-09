using ECommerceAPI.Domain;
using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerceAPI.Controller;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase {
    private readonly ECommerceApiDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IMediator _mediator;
    public UserController(ECommerceApiDbContext context, IJwtService jwtService, IMediator mediator) {
        _context = context;
        _jwtService = jwtService;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponse>> RegisterUser(RegisterUserDto dto) {
        return Created(string.Empty, await _mediator.Send(new CreateUserCommand(dto)));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUser(LoginUserDto dto) {
        var theUser = await _mediator.Send(new UserLoginCommand(dto));

        var accessToken = _jwtService.Generate_JWT(theUser);
        var refreshToken = _jwtService.Generate_RefreshToken(theUser.Id.ToString());

        _context.RefreshTokenTable.Add(refreshToken);
        await _context.SaveChangesAsync();

        Response.Cookies.Append("RefreshToken", refreshToken.Token, new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = refreshToken.ExpiresAt
        });

        return Ok(new {Token = accessToken});
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        var valueOfRefreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(valueOfRefreshToken))
            return Unauthorized();

        var kyuuRefreshToken = await _context.RefreshTokenTable.SingleOrDefaultAsync(n => n.Token == valueOfRefreshToken);

        if (kyuuRefreshToken == null || !kyuuRefreshToken.IsActive)
            return Unauthorized();

        kyuuRefreshToken.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = _jwtService.Generate_RefreshToken(kyuuRefreshToken.UserId);
        _context.RefreshTokenTable.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        var um = await _context.UserTable.SingleOrDefaultAsync(n => n.Id == int.Parse(kyuuRefreshToken.UserId));
        var newAccessToken = _jwtService.Generate_JWT(um);

        Response.Cookies.Append("RefreshToken", newRefreshToken.Token, new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = newRefreshToken.ExpiresAt
        });

        return Ok(new { Token = newAccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        var valueOfRefreshToken = Request.Cookies["RefreshToken"];

        if (!string.IsNullOrEmpty(valueOfRefreshToken)) {
            var kyuuRefreshToken = await _context.RefreshTokenTable.SingleOrDefaultAsync(n => n.Token == valueOfRefreshToken);
            if (kyuuRefreshToken != null) {
                kyuuRefreshToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        Response.Cookies.Delete("RefreshToken");
        return NoContent();
    }
    
}