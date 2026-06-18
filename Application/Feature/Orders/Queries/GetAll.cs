using MediatR;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetOneUsersAllOrders {
    public record GetOneUsersAllOrdersQuery(int UserId) : IRequest<IEnumerable<OrderDto>>;

    public class Handler : IRequestHandler<GetOneUsersAllOrdersQuery, IEnumerable<OrderDto>> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;         
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOneUsersAllOrdersQuery query, CancellationToken cancellationToken) {
            IEnumerable<Order> theOrderRisuto = await _context.OrderTable
                .AsNoTracking()
                .Include(x => x.OrderItemRisuto)
                .ThenInclude(x => x.Product)
                .Where(x => x.UserId == query.UserId)
                .ToListAsync(cancellationToken);

            if (!theOrderRisuto.Any())
                throw new NotFoundException("Order not found");

            return theOrderRisuto.Select(x => new OrderDto {
                OrderId = x.Id,
                AllCartItems = new AllCartItemsDto(x.OrderItemRisuto.Select(i => new SingleCartItemDto {
                    //ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductDescription = i.Product.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList()),
                OrderStatus = x.OrderStatus,
                CreatedAt = x.CreatedAt,
                PaymentIntenId = x.PaymentIntentId
            });
        }
    }

}
