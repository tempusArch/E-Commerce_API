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

    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public OrderController(IMediator mediator, IHttpContextService httpContextService) {    
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadOrderDto>>> GetOneUsersAllOrders() {
        var userId = _httpContextService.GetCurrentUserId();

        return Ok(await _mediator.Send(new GetOneUsersAllOrdersQuery(userId)));
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<ReadOrderDto>> GetSingleOrderById(int orderId) {
        var userId = _httpContextService.GetCurrentUserId();

        return Ok(await _mediator.Send(new GetOneOrderQuery(userId, orderId)));
    }

    [HttpPost]
    public async Task<ActionResult<ReadOrderDto>> CreateOrder() {
        var userId = _httpContextService.GetCurrentUserId();

        var result = await _mediator.Send(new CreateOrderCommand(userId));

        return Created(string.Empty, result);
    }

    [HttpPost("{orderId}")]
    public async Task<IActionResult> CancelOrder(int orderId) {
        var userId = _httpContextService.GetCurrentUserId();

        await _mediator.Send(new CancelOrderCommand(userId, orderId));

        return NoContent();
    }

}