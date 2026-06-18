using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetAllCartItems {
    public record GetAllCartItemsQuery(int CartId) : IRequest<AllCartItemsDto>;

    public class Handler : IRequestHandler<GetAllCartItemsQuery, AllCartItemsDto> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;         
        }

        public async Task<AllCartItemsDto> Handle(GetAllCartItemsQuery query, CancellationToken cancellationToken) {
            var result = await _context.CartItemTable
                .AsNoTracking()
                .Where(x => x.CartId == query.CartId)
                .Select(x => new SingleCartItemDto {
                    //ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductDescription = x.Product.Description,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.Price
                }).ToListAsync(cancellationToken);

            if (!result.Any())
                throw new NotFoundException("Cart item not found");

            return new AllCartItemsDto(result);
        }
    }
}
