using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class DeleteCartItem {
    public record DeleteCartItemCommand(int ProductId, int CartId) : IRequest<Unit>;

    public class Handler : IRequestHandler<DeleteCartItemCommand, Unit> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;         
        }

        public async Task<Unit> Handle(DeleteCartItemCommand command, CancellationToken cancellationToken) {
            var theOne = await _context.CartItemTable.FirstOrDefaultAsync(
                x => x.ProductId == command.ProductId && x.CartId == command.CartId, cancellationToken);

            if (theOne == null)
                throw new NotFoundException("Cart item not found");

            _context.CartItemTable.Remove(theOne);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
