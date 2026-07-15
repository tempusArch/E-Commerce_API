using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using Stripe;
using Azure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ECommerceAPI.Application;

public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, string> {
    private readonly ECommerceApiDbContext _context;
    public CreatePaymentIntentHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<string> Handle(CreatePaymentIntentCommand command, CancellationToken cancellationToken) {
        var theOrder = await _context.OrderTable
            .AsNoTracking()
            .FirstOrDefaultAsync(o => 
                o.UserId == command.UserId && 
                o.Id == command.OrderId, 
                cancellationToken
            );

        if (theOrder == null)
            throw new NotFoundException("Order not found");

        decimal totalAmount = theOrder.TotalPrice;
        var paymentIntentService = new PaymentIntentService();

        var paymentIntent = await paymentIntentService.CreateAsync(
            new PaymentIntentCreateOptions {
                Amount = (long)(totalAmount * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> {"card"},
                
                Metadata = new Dictionary<string, string> {
                    {"order_id", command.OrderId.ToString()}
                }
            }, 
            cancellationToken: cancellationToken);

        theOrder.PaymentIntentId = paymentIntent.Id;
        theOrder.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return paymentIntent.ClientSecret;
    }
}
