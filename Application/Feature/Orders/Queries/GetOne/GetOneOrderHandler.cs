using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class GetOneOrderHandler : IRequestHandler<GetOneOrderQuery, OrderDto> {
    private readonly ECommerceApiDbContext _context;
    public GetOneOrderHandler(ECommerceApiDbContext context) {
        _context = context;         
    }    

    public async Task<OrderDto> Handle(GetOneOrderQuery query, CancellationToken cancellationToken) {
        var theOrder = await _context.OrderTable
            .AsNoTracking()
            .Include(x => x.OrderItemRisuto)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == query.OrderId && x.UserId == query.UserId, cancellationToken);

        if (theOrder == null)
            throw new NotFoundException("Order not found");

        List<SingleCartItemDto> carItemRisuto = theOrder.OrderItemRisuto
            .Select(x => new SingleCartItemDto {
                //ProductId = x.ProductId,
                ProductName = x.Product.Name,
                ProductDescription = x.Product.Description,
                Quantity = x.Quantity,
                UnitPrice = x.Product.Price
            })
            .ToList();

        return new OrderDto {
            //Id = theOrder.Id,
            AllCartItems = new AllCartItemsDto(carItemRisuto),
            CreatedAt = theOrder.CreatedAt,
            OrderStatus = theOrder.OrderStatus,
            PaymentIntentId = theOrder.PaymentIntentId
        };
    }
}
