using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UserLogin {
    public record UserLoginCommand(LoginUserDto LoginUserDto) : IRequest<User>;

    public class Handler : IRequestHandler<UserLoginCommand, User> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        public Handler(ECommerceApiDbContext context, IMapper mapper, IJwtService jwtService, IPasswordHasher passordHasher) {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            _passwordHasher = passordHasher;
        }

        public async Task<User> Handle(UserLoginCommand command, CancellationToken cancellationToken) {
            var theUser = await _context.UserTable
                .SingleOrDefaultAsync(x => x.Email == command.LoginUserDto.Email);

            //if (theUser == null || !_passwordHasher.VerifyPassword(command.LoginUserDto.Password, theUser.PasswordHashed)) 
                //throw new UnauthorizedAccessException();

            if (theUser == null) {
                Console.WriteLine("❌ User not found");
                throw new UnauthorizedAccessException("User not found");
            }

            var isValidPassword = _passwordHasher.VerifyPassword(
                command.LoginUserDto.Password,
                theUser.PasswordHashed
            );

            Console.WriteLine($"Password valid: {isValidPassword}");

            if (!isValidPassword) 
                throw new UnauthorizedAccessException("Invalid password");
          
            return theUser;
                
        }
    }
}