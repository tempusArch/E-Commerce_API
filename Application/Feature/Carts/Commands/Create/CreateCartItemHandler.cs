using ECommerceAPI.Domain;
using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class CreateCartItemHandler : IRequestHandler<CreateCartItemCommand, ReadCartItemDto> {
    private readonly ECommerceApiDbContext _context;
    public CreateCartItemHandler(ECommerceApiDbContext context) {
        _context = context;         
    }
        
    public async Task<ReadCartItemDto> Handle(CreateCartItemCommand command, CancellationToken cancellationToken) {
        var theProduct = await _context.ProductTable
            .AsNoTracking()
            .Where(p => p.Id == command.ProductId)
            .Select(p => new {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Quantity
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (theProduct == null)
            throw new NotFoundException("Product not found");

        if (theProduct.Quantity < command.Quantity)
            throw new InvalidOperationException($"Only {theProduct.Quantity} items of this product available");

        var cartId = await _context.CartTable
            .Where(s => s.UserId == command.UserId)
            .Select(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var theCartItem = await _context.CartItemTable
            .FirstOrDefaultAsync(x => 
                x.ProductId == command.ProductId && 
                x.UserId == command.UserId, 
                cancellationToken
            );

        var finalQuantity = command.Quantity;

        if (theCartItem == null) {
            var newOne = new CartItem {
                ProductId = command.ProductId,
                CartId = cartId,
                Quantity = command.Quantity,
                UserId = command.UserId
            };

            _context.CartItemTable.Add(newOne);
        } else {
            theCartItem.Quantity += command.Quantity;
            finalQuantity = theCartItem.Quantity;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new ReadCartItemDto {
            ProductId = theProduct.Id,
            ProductName = theProduct.Name,
            ProductDescription = theProduct.Description,
            Quantity = finalQuantity,
            UnitPrice = theProduct.Price
        };
    }
}

