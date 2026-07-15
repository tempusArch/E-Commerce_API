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
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public UserController(IMediator mediator, IHttpContextService httpContextService) {
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponse>> RegisterUser(RegisterUserDto dto) {
        return Created(string.Empty, await _mediator.Send(new CreateUserCommand(dto)));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUser(LoginUserDto dto) {
        var accessToken = await _mediator.Send(new LoginUserCommand(dto));

        return Ok(new {Token = accessToken});
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh() {
        _httpContextService.CheckUserIdClaim();

        var newAccessToken = await _mediator.Send(new RefreshUserCommand());

        return Ok(new { Token = newAccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        _httpContextService.CheckUserIdClaim();

        await _mediator.Send(new LogoutUserCommand());
        
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetOneUser(int id) {
        _httpContextService.CheckUserIdClaim();

        return Ok(await _mediator.Send(new GetOneUserQuery(id)));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<UserListResponse>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int limit = 10) {
        _httpContextService.CheckUserIdClaim();

        return Ok(await _mediator.Send(new GetAllUsersQuery(page, limit)));
    }
    
}