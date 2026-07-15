using MediatR;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetOneUsersAllOrdersHandler : IRequestHandler<GetOneUsersAllOrdersQuery, OrderListResponse> {
    private readonly ECommerceApiDbContext _context;
    public GetOneUsersAllOrdersHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<OrderListResponse> Handle(GetOneUsersAllOrdersQuery query, CancellationToken cancellationToken) {
        var result = await _context.OrderTable
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId)
            .Select(x => new ReadOrderDto {
                OrderId = x.Id,
                TotalPrice = x.TotalPrice,
                OrderStatus = x.OrderStatus,

                OrderItemRisuto = x.OrderItemRisuto
                    .Select(z => new ReadOrderItemDto {
                        ProductId = z.ProductId,
                        ProductName = z.Product.Name,
                        ProductDescription = z.Product.Description,
                        
                        UnitPrice = z.UnitPrice,
                        Quantity = z.Quantity,
                        
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return new OrderListResponse {Items = result};
    }
}