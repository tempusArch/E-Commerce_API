using MediatR;

namespace ECommerceAPI.Application;

public record CancelOrderCommand(int UserId, int OrderId) : IRequest<Unit>;