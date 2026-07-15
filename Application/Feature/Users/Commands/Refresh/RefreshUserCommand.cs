using MediatR;

namespace ECommerceAPI.Application;

public record RefreshUserCommand : IRequest<string>;