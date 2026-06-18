using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using Stripe;

namespace ECommerceAPI.Application;

public class CreateOrder {
    public record CreateOrderCommand(int UserId, int CartId) : IRequest<OrderDto>;

    public class Handler : IRequestHandler<CreateOrderCommand, OrderDto> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;         
        }

        public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken) {
            

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var allCartItems = await _context.CartItemTable
                .Include(x => x.Product)
                .Where(x => x.CartId == command.CartId)
                .ToListAsync(cancellationToken);

                if (allCartItems.Count == 0)
                    throw new NotFoundException("Your cart is empty");

                foreach (var i in allCartItems) {
                    if (i.Product.Quantity < i.Quantity) {
                        throw new InvalidOperationException($"Only {i.Product.Quantity} items of {i.Product.Name} available");
                    }

                    i.Product.Quantity -= i.Quantity;
                }

                var newOrder = new Order {
                    UserId = command.UserId,
                };

                _context.OrderTable.Add(newOrder);

                var orderItemRisuto = allCartItems.Select(x => new OrderItem {
                    Order = newOrder,
                    Product = x.Product,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,            
                });

                _context.CartItemTable.RemoveRange(allCartItems);
                _context.OrderItemTable.AddRange(orderItemRisuto);

                List<SingleCartItemDto> allCartItemsDtoRisuto = allCartItems.Select(x => new SingleCartItemDto {
                    //ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductDescription = x.Product.Description,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.Price
                }).ToList();

                var allCartItemsDto = new AllCartItemsDto(allCartItemsDtoRisuto);
                var totalAmount = allCartItemsDto.TotalPrice;

                newOrder.TotalPrice = allCartItemsDto.TotalPrice;

                var result = new OrderDto {
                    OrderId = newOrder.Id,
                    AllCartItems = allCartItemsDto
                };

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

}
