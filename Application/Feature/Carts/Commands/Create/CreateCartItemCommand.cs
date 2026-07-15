using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record CreateCartItemCommand(int UserId, int ProductId, int Quantity) : IRequest<ReadCartItemDto>;