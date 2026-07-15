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
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public CartController(IMediator mediator, IHttpContextService httpContextService) {     
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    [HttpGet]
    public async Task<ActionResult<CartItemListResponse>> GetTheWholeCart() {
        var userId = _httpContextService.GetCurrentUserId();

        return Ok(await _mediator.Send(new GetAllCartItemsQuery(userId)));
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ReadCartItemDto>> GetSingleCartItem(int productId) {
        var userId = _httpContextService.GetCurrentUserId();

        return Ok(await _mediator.Send(new GetOneCartItemQuery(userId, productId)));
    }

    [HttpPost("{productId}")]
    public async Task<ActionResult<ReadCartItemDto>> CreateCartItem(int productId, [FromQuery] int quantity = 1) {
        var userId = _httpContextService.GetCurrentUserId();

        var result = await _mediator.Send(new CreateCartItemCommand(userId, productId, quantity));

        return Created(string.Empty, result);
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult<ReadCartItemDto>> UpdateCartItem(int productId, [FromQuery] int quantity) {
        var userId = _httpContextService.GetCurrentUserId();

        return Ok(await _mediator.Send(new UpdateCartItemCommand(userId, productId, quantity)));
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteCartItem(int productId) {
        var userId = _httpContextService.GetCurrentUserId();

        await _mediator.Send(new DeleteCartItemCommand(userId, productId));

        return NoContent();
    }
}