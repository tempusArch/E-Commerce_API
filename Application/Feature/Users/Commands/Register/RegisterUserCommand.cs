using MediatR;

namespace ECommerceAPI.Application;

public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequest<UserResponse>;