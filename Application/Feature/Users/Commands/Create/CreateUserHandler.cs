using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using FluentResults;

namespace ECommerceAPI.Application;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    public CreateUserHandler(ECommerceApiDbContext context, IMapper mapper, IJwtService jwtService, IPasswordHasher passordHasher) {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
        _passwordHasher = passordHasher;
    }

    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken) {
        var isEmailExisted = await _context.UserTable
            .AnyAsync(x => x.Email == command.RegisterUserDto.Email);

        if (isEmailExisted)
            throw new InvalidOperationException("Email already existed");

        User theNewUser = _mapper.Map<User>(command.RegisterUserDto);
            
        theNewUser.Cart = new Cart {
            User = theNewUser
        };
        theNewUser.PasswordHashed = _passwordHasher.HashPassword(command.RegisterUserDto.Password);

        _context.UserTable.Add(theNewUser);
        await _context.SaveChangesAsync();

        return new UserResponse {
            Name = theNewUser.Name,
            Email = theNewUser.Email
        };
    }
}
