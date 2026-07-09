using MediatR;

namespace ECommerceAPI.Application;

public record GetAllCartItemsQuery(int CartId) : IRequest<AllCartItemsDto>;