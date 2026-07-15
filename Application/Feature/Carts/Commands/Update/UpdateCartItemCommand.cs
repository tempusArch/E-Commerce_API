using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record UpdateCartItemCommand(int UserId, int ProductId, int Quantity) : IRequest<ReadCartItemDto>;
