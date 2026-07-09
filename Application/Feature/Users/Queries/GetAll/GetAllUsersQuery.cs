using MediatR;

namespace ECommerceAPI.Application;

public record GetAllUsersQuery(int Page, int Limit) : IRequest<UserListResponse>;