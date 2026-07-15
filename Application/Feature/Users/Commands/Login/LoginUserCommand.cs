using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record LoginUserCommand(LoginUserDto LoginUserDto) : IRequest<string>;