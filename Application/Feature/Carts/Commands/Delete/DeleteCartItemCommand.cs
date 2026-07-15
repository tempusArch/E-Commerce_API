using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record DeleteCartItemCommand(int UserId, int ProductId) : IRequest<Unit>;