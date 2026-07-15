using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class GetOneOrderHandler : IRequestHandler<GetOneOrderQuery, ReadOrderDto> {
    private readonly ECommerceApiDbContext _context;
    public GetOneOrderHandler(ECommerceApiDbContext context) {
        _context = context;         
    }    

    public async Task<ReadOrderDto> Handle(GetOneOrderQuery query, CancellationToken cancellationToken) {
        var theOne = await _context.OrderTable
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId && x.Id == query.OrderId)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (theOne == null)
            throw new NotFoundException("Order not found");

        return theOne;
        
    }
}
