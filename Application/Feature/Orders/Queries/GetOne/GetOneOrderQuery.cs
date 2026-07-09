using MediatR;

namespace ECommerceAPI.Application;

public record GetOneOrderQuery(int UserId, int OrderId) : IRequest<OrderDto>;