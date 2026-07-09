using MediatR;

namespace ECommerceAPI.Application;

public record GetOneCartItemQuery(int ProductId, int CartId) : IRequest<SingleCartItemDto>;