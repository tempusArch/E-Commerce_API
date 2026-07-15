using ECommerceAPI.Domain;
using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Stripe;

namespace ECommerceAPI.Controller;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase {
    private readonly ECommerceApiDbContext _context;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public PaymentController(ECommerceApiDbContext context, IMediator mediator, IHttpContextService httpContextService) {
        _context = context;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    [HttpPost("create-paymentIntent/{orderId}")]
    public async Task<ActionResult<string>> CreatePaymentIntent(int orderId) {
        var userId = _httpContextService.GetCurrentUserId();

        var result = await _mediator.Send(new CreatePaymentIntentCommand(userId, orderId));
        return Created(string.Empty, result);
    }


    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook() {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        Event stripeEvent;
        try {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                "Your_Webhook_Secret"
            );
        } catch (StripeException e) {
            return BadRequest($"Webhook Error: {e.Message}");
        }
        

        /*switch (stripeEvent.Type) {
            case "payment_intent.succeeded":
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                await HandlePaymentIntentSucceeded(paymentIntent);
                break;

            case "payment_intent.payment_failed":
                var failedIntent = stripeEvent.Data.Object as PaymentIntent;
                await HandlePaymentFailed(failedIntent);
                break;

            default:
                // ignore other events
                break;
        }*/

        return Ok(); 
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent intent) {
        var orderId = intent.Metadata["orderId"];
        var order = await _context.OrderTable.FindAsync(int.Parse(orderId));

        if (order == null) 
            return;

        order.OrderStatus = OrderStatus.Paid;
        order.PaymentIntentId = intent.Id;

        await _context.SaveChangesAsync();
    }

    private async Task HandlePaymentFailed(PaymentIntent intent) {
        var orderId = intent.Metadata["orderId"];
        var order = await _context.OrderTable.FindAsync(int.Parse(orderId));
        
        if (order == null)
            return;

        //order.OrderStatus = OrderStatus.Failed;

        await _context.SaveChangesAsync();
    }
}