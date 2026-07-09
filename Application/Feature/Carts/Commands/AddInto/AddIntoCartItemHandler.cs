using ECommerceAPI.Domain;
using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class AddIntoCartItemHandler : IRequestHandler<AddIntoCartItemCommand, SingleCartItemDto> {
    private readonly ECommerceApiDbContext _context;
    public AddIntoCartItemHandler(ECommerceApiDbContext context) {
        _context = context;         
    }
        
    public async Task<SingleCartItemDto> Handle(AddIntoCartItemCommand command, CancellationToken cancellationToken) {
        Product? theProduct = await _context.ProductTable
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == command.CartItem.ProductId, cancellationToken);

        if (theProduct == null)
            throw new NotFoundException("Product not found");

        if (theProduct.Quantity < command.CartItem.Quantity)
            throw new InvalidOperationException($"Only {theProduct.Quantity} items of this product available");

        CartItem? theCartItem = await _context.CartItemTable
            .FirstOrDefaultAsync(x => x.ProductId == command.CartItem.ProductId && x.CartId == command.CartItem.CartId, cancellationToken);

        if (theCartItem == null) {
            theCartItem = command.CartItem;
            _context.CartItemTable.Add(theCartItem);
        } else
            theCartItem.Quantity = command.CartItem.Quantity;

        await _context.SaveChangesAsync(cancellationToken);

        return new SingleCartItemDto {
            //ProductId = theProduct.Id,
            ProductName = theProduct.Name,
            ProductDescription = theProduct.Description,
            Quantity = command.CartItem.Quantity,
            UnitPrice = theProduct.Price
        };
    }
}

