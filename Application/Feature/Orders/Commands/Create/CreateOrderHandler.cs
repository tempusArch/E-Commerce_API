using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using Stripe;

namespace ECommerceAPI.Application;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ReadOrderDto> {
    private readonly ECommerceApiDbContext _context;
    public CreateOrderHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<ReadOrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken) {
        var theOrderingOnes = await _context.CartItemTable
            .Include(x => x.Product)
            .Where(x => x.UserId == command.UserId)
            .ToListAsync(cancellationToken);

        if (!theOrderingOnes.Any())
            throw new NotFoundException("Your cart is empty");

        foreach (var i in theOrderingOnes) {
            if (i.Product.Quantity < i.Quantity) {
                throw new InvalidOperationException($"Only {i.Product.Quantity} items of {i.Product.Name} available");
            }

            i.Product.Quantity -= i.Quantity;
        }

        var totalPrice = theOrderingOnes.Sum(x => x.TotalPrice);

        var newOrder = new Order {
            UserId = command.UserId,
            TotalPrice = totalPrice
        };

        var newOrderItemRisuto = theOrderingOnes
            .Select(x => new OrderItem {
                Order = newOrder,
                ProductId = x.ProductId,
                UnitPrice = x.Product.Price,
                Quantity = x.Quantity,
                UserId = x.UserId           
            })
            .ToList();

        _context.OrderTable.Add(newOrder);
        _context.CartItemTable.RemoveRange(theOrderingOnes);
        _context.OrderItemTable.AddRange(newOrderItemRisuto);

        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.OrderTable
            .AsNoTracking()
            .Where(x => x.UserId == command.UserId && x.Id == newOrder.Id)
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
                    .ToList(),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
                
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            throw new InvalidOperationException("Order creation failed");

        return result;
    }
}
