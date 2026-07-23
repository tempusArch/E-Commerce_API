using ECommerceAPI.Domain;
using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ECommerceAPI.Application;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Unit> {
    private readonly ECommerceApiDbContext _context;
    public CancelOrderHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<Unit> Handle(CancelOrderCommand command, CancellationToken cancellationToken) {
        var theOrder = await _context.OrderTable
            .Include(x => x.OrderItemRisuto)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == command.OrderId && x.UserId == command.UserId);

        if (theOrder == null)
            throw new NotFoundException("Order not found");

        if (theOrder.OrderStatus == OrderStatus.Shipping)
            throw new InvalidOperationException("Can not cancel order as it is shipping");

        if (!string.IsNullOrEmpty(theOrder.PaymentIntentId)) {
            var refundService = new RefundService();

            var refundOptions = new RefundCreateOptions() {
                PaymentIntent = theOrder.PaymentIntentId,
                Amount = (long)(theOrder.TotalPrice * 100),
                Reason = "requested_by_customer"
            };

            await refundService.CreateAsync(refundOptions, cancellationToken: cancellationToken);
        }

        foreach (var i in theOrder.OrderItemRisuto)
            i.Product.Quantity += i.Quantity;

        _context.OrderItemTable.RemoveRange(theOrder.OrderItemRisuto);
        theOrder.OrderStatus = OrderStatus.Cancelled;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}