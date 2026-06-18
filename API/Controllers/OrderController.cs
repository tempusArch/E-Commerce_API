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
public class OrderController : ControllerBase {
    private readonly ECommerceApiDbContext _context;
    private readonly IMediator _mediator;
    public OrderController(ECommerceApiDbContext context, IMediator mediator) {
        _context = context; 
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOneUsersAllOrders() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        return Ok(await _mediator.Send(new GetOneUsersAllOrders.GetOneUsersAllOrdersQuery(int.Parse(userId))));
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderDto>> GetSingleOrderById(int orderId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        return Ok(await _mediator.Send(new GetSingleOrder.GetSingleOrderQuery(int.Parse(userId), orderId)));
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        Cart? cart = await _context.CartTable
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == int.Parse(userId));

        var result = await _mediator.Send(new CreateOrder.CreateOrderCommand(int.Parse(userId), cart.Id));

        return Created(string.Empty, result);
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID claim missing");

        await _mediator.Send(new DeleteOrder.DeleteOrderCommand(int.Parse(userId), orderId));

        return NoContent();
    }

}