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
    public AdminController(ECommerceApiDbContext context, IMediator mediator) {
        _context = context;
        _mediator = mediator;
    }

    [HttpPost("product")]
    public async Task<ActionResult<ReadProductDto>> CreateProdut(CreateProductDto dto) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        var result = await _mediator.Send(new CreateProductCommand(dto));

        return Created(string.Empty, result);
    }

    [HttpPut("product/{productId}")]
    public async Task<ActionResult<ReadProductDto>> UpdateProduct(int productId, UpdateProductDto dto) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        dto.Id = productId;
        return Ok(await _mediator.Send(new UpdateProductCommand(dto)));
    }

    [HttpDelete("product/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");
                    
        await _mediator.Send(new DeleteProductCommand(productId));

        return NoContent();
    }

    [HttpPost("category")]
    public async Task<ActionResult<ReadCategoryDto>> CreateCategory([FromQuery] string categoryName) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        var result = await _mediator.Send(new CreateCategoryCommand(categoryName));

        return Created(string.Empty, result);
    }

    [HttpPut("category/{categoryId}")]
    public async Task<ActionResult<ReadCategoryDto>> UpdateCategory(int categoryId, [FromQuery] string categoryName) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        return Ok(await _mediator.Send(new UpdateCategoryCommand(categoryId, categoryName)));
    }

    [HttpDelete("category/{categoryId}")]
    public async Task<IActionResult> DeleteCategory(int categoryId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");
                    
        await _mediator.Send(new DeleteCategoryCommand(categoryId));

        return NoContent();
    }

    [HttpGet("user/{Id}")]
    public async Task<ActionResult<User>> GetOneUser(int Id) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        return Ok(await _mediator.Send(new GetOneUserQuery(Id)));
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserListResponse>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int limit = 10) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        return Ok(await _mediator.Send(new GetAllUsersQuery(page, limit)));
    }
}