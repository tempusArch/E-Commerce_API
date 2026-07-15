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
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase {
    private readonly ECommerceApiDbContext _context;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public AdminController(ECommerceApiDbContext context, IMediator mediator, IHttpContextService httpContextService) {
        _context = context;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    [HttpPost("product")]
    public async Task<ActionResult<ReadProductDto>> CreateProdut(CreateProductDto dto) {
        _httpContextService.CheckUserIdClaim();

        var result = await _mediator.Send(new CreateProductCommand(dto));

        return Created(string.Empty, result);
    }

    [HttpPut("product/{productId}")]
    public async Task<ActionResult<ReadProductDto>> UpdateProduct(int productId, UpdateProductDto dto) {
        _httpContextService.CheckUserIdClaim();

        return Ok(await _mediator.Send(new UpdateProductCommand(productId, dto)));
    }

    [HttpDelete("product/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId) {
        _httpContextService.CheckUserIdClaim();
                    
        await _mediator.Send(new DeleteProductCommand(productId));

        return NoContent();
    }

    [HttpPost("category")]
    public async Task<ActionResult<ReadCategoryDto>> CreateCategory([FromQuery] string categoryName) {
        _httpContextService.CheckUserIdClaim();

        var result = await _mediator.Send(new CreateCategoryCommand(categoryName));

        return Created(string.Empty, result);
    }

    [HttpPut("category/{categoryId}")]
    public async Task<ActionResult<ReadCategoryDto>> UpdateCategory(int categoryId, [FromQuery] string categoryName) {
        _httpContextService.CheckUserIdClaim();

        return Ok(await _mediator.Send(new UpdateCategoryCommand(categoryId, categoryName)));
    }

    [HttpDelete("category/{categoryId}")]
    public async Task<IActionResult> DeleteCategory(int categoryId) {
        _httpContextService.CheckUserIdClaim();
                    
        await _mediator.Send(new DeleteCategoryCommand(categoryId));

        return NoContent();
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<User>> GetOneUser(int Id) {
        _httpContextService.CheckUserIdClaim();

        return Ok(await _mediator.Send(new GetOneUserQuery(Id)));
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserListResponse>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int limit = 10) {
        _httpContextService.CheckUserIdClaim();

        return Ok(await _mediator.Send(new GetAllUsersQuery(page, limit)));
    }
}