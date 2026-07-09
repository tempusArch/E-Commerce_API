using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record AddIntoCartItemCommand(CartItem CartItem) : IRequest<SingleCartItemDto>;
