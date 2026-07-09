using MediatR;

namespace ECommerceAPI.Application;

public record CreateUserCommand(RegisterUserDto RegisterUserDto) : IRequest<UserResponse>;