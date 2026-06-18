using ECommerceAPI.Domain;
using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ECommerceAPI.Application;

public class DeleteOrder {
    public record DeleteOrderCommand(int UserId, int OrderId) : IRequest<Unit>;

    public class Handler : IRequestHandler<DeleteOrderCommand, Unit> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;         
        }

        public async Task<Unit> Handle(DeleteOrderCommand command, CancellationToken cancellationToken) {
            var theOrder = await _context.OrderTable
                .Include(x => x.OrderItemRisuto)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == command.OrderId && x.UserId == command.UserId);

            if (theOrder == null)
                throw new NotFoundException("Order not found");

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
            _context.OrderTable.Remove(theOrder);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
