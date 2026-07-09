using MediatR;

namespace ECommerceAPI.Application;

public record CreateOrderCommand(int UserId, int CartId) : IRequest<OrderDto>;