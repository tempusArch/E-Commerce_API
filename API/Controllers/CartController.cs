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
public class CartController : ControllerBase {
    private readonly ECommerceApiDbContext _context;
    private readonly IMediator _mediator;
    public CartController(ECommerceApiDbContext context, IMediator mediator) {
        _context = context; 
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<AllCartItemsDto>> GetTheWholeCart() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        Cart? cart = await _context.CartTable
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

        return Ok(await _mediator.Send(new GetAllCartItems.GetAllCartItemsQuery(cart.Id)));
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<SingleCartItemDto>> GetSingleCartItem(int productId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        Cart? cart = await _context.CartTable
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

        return Ok(await _mediator.Send(new GetSingleCartItem.GetSingleCartItemQuery(productId, cart.Id)));
    }

    [HttpPost("{productId}")]
    public async Task<ActionResult<SingleCartItemDto>> AddIntoCartItem(int productId, [FromQuery] int quantity = 1) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        Cart? cart = await _context.CartTable
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

        var result = await _mediator.Send(new AddIntoCartItem.AddIntoCartItemCommand(new CartItem {
            ProductId = productId,
            CartId = cart.Id,
            Quantity = quantity
        }));

        return Created(string.Empty, result);
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult<SingleCartItemDto>> UpdateCartItem(int productId, [FromQuery] int quantity) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        Cart? cart = await _context.CartTable
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

        return Ok(await _mediator.Send(new UpdateCartItem.UpdateSingleCartItemCommand(new CartItem {
            ProductId = productId,
            CartId = cart.Id,
            Quantity = quantity
        })));
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteCartItem(int productId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        Cart? cart = await _context.CartTable
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

        await _mediator.Send(new DeleteCartItem.DeleteCartItemCommand(productId, cart.Id));

        return NoContent();
    }
}