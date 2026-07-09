using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetOneCartItemHandler : IRequestHandler<GetOneCartItemQuery, SingleCartItemDto> {
    private readonly ECommerceApiDbContext _context;
    public GetOneCartItemHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<SingleCartItemDto> Handle(GetOneCartItemQuery query, CancellationToken cancellationToken) {
        var theOne = await _context.CartItemTable
            .AsNoTracking()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.ProductId == query.ProductId && 
                x.CartId == query.CartId, 
                cancellationToken
            );

        if (theOne == null)
            throw new NotFoundException("Cart item not found");

        return new SingleCartItemDto {
            //ProductId = theOne.ProductId,
            ProductName = theOne.Product.Name,
            ProductDescription = theOne.Product.Description,
            Quantity = theOne.Quantity,
            UnitPrice = theOne.Product.Price
        };
    }
}

