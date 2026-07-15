using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record LogoutUserCommand : IRequest<Unit>;