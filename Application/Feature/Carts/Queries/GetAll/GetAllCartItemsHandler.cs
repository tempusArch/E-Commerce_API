using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetAllCartItemsHandler : IRequestHandler<GetAllCartItemsQuery, CartItemListResponse> {
    private readonly ECommerceApiDbContext _context;
    public GetAllCartItemsHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<CartItemListResponse> Handle(GetAllCartItemsQuery query, CancellationToken cancellationToken) {
        var result = await _context.CartItemTable
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId)
            .Select(x => new ReadCartItemDto {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                ProductDescription = x.Product.Description,
                Quantity = x.Quantity,
                UnitPrice = x.Product.Price
            })
            .ToListAsync(cancellationToken);

        return new CartItemListResponse{ Items = result };
    }
}

