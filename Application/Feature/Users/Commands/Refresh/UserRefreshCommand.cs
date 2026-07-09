using MediatR;

namespace ECommerceAPI.Application;

public record UserRefreshCommand(string valueOfRefreshToken) : IRequest<string>;