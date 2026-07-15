using MediatR;

namespace ECommerceAPI.Application;

public record CreateOrderCommand(int UserId) : IRequest<ReadOrderDto>;