using MediatR;

namespace ECommerceAPI.Application;

public record GetAllCartItemsQuery(int UserId) : IRequest<CartItemListResponse>;