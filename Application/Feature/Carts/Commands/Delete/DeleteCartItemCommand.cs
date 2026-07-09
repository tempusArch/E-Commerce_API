using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record DeleteCartItemCommand(int ProductId, int CartId) : IRequest<Unit>;