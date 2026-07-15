using MediatR;

namespace ECommerceAPI.Application;

public record GetOneUsersAllOrdersQuery(int UserId) : IRequest<OrderListResponse>;