using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetOneCartItemHandler : IRequestHandler<GetOneCartItemQuery, ReadCartItemDto> {
    private readonly ECommerceApiDbContext _context;
    public GetOneCartItemHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<ReadCartItemDto> Handle(GetOneCartItemQuery query, CancellationToken cancellationToken) {

        var theOne = await _context.CartItemTable
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId && x.ProductId == query.ProductId)
            .Select(x => new ReadCartItemDto {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                ProductDescription = x.Product.Description,
                Quantity = x.Quantity,
                UnitPrice = x.Product.Price
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (theOne == null)
            throw new NotFoundException("Cart item not found");

        return theOne;
    }
}

