using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record UpdateCartItemCommand(CartItem CartItem) : IRequest<SingleCartItemDto>;
