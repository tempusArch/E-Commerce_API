using MediatR;

namespace ECommerceAPI.Application;

public record GetOneCartItemQuery(int UserId, int ProductId) : IRequest<ReadCartItemDto>;