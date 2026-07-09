using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record UserLoginCommand(LoginUserDto LoginUserDto) : IRequest<User>;