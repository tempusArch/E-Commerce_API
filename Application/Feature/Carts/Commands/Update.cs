using ECommerceAPI.Domain;
using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class UpdateCartItem {
    public record UpdateSingleCartItemCommand(CartItem CartItem) : IRequest<SingleCartItemDto>;

    public class Handler : IRequestHandler<UpdateSingleCartItemCommand, SingleCartItemDto> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;         
        }

        public async Task<SingleCartItemDto> Handle(UpdateSingleCartItemCommand command, CancellationToken cancellationToken) {
            CartItem? theOne = await _context.CartItemTable
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ProductId == command.CartItem.ProductId &&
                x.CartId == command.CartItem.CartId,
                cancellationToken);

            if (theOne == null)
                throw new NotFoundException("Cart item not found");

            theOne.Quantity = command.CartItem.Quantity;
            await _context.SaveChangesAsync(cancellationToken);

            return new SingleCartItemDto {
                //ProductId = theOne.ProductId,
                ProductName = theOne.Product.Name,
                ProductDescription = theOne.Product.Description,
                Quantity = theOne.Quantity,
                UnitPrice = theOne.Product.Price
            };
        }
    }
}
