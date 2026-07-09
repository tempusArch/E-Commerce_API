using MediatR;

namespace ECommerceAPI.Application;

public record DeleteOrderCommand(int UserId, int OrderId) : IRequest<Unit>;