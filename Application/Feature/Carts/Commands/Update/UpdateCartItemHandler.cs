using ECommerceAPI.Domain;
using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;


public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, ReadCartItemDto> {
    private readonly ECommerceApiDbContext _context;
    public UpdateCartItemHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<ReadCartItemDto> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken) {
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
        
        var theOne = await _context.CartItemTable
            .FirstOrDefaultAsync(x => 
                x.ProductId == command.ProductId &&
                x.UserId == command.UserId,
                cancellationToken
            );

        if (theOne == null)
            throw new NotFoundException("Cart item not found");

        theOne.Quantity = command.Quantity;
        await _context.SaveChangesAsync(cancellationToken);

        return new ReadCartItemDto {
            ProductId = theProduct.Id,
            ProductName = theProduct.Name,
            ProductDescription = theProduct.Description,
            Quantity = theOne.Quantity,
            UnitPrice = theProduct.Price
        };
    }
}

