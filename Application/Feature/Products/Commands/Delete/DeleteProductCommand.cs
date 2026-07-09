using MediatR;

namespace ECommerceAPI.Application;

public record DeleteProductCommand(int Id) : IRequest<Unit>;