using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, string> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
     private readonly IHttpContextService _httpContextService;
    public LoginUserHandler(ECommerceApiDbContext context, IMapper mapper, IJwtService jwtService, IPasswordHasher passordHasher, IHttpContextService httpContextService) {
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
        _passwordHasher = passordHasher;
        _httpContextService = httpContextService;
    }

    public async Task<string> Handle(LoginUserCommand command, CancellationToken cancellationToken) {
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
          
        var accessToken = _jwtService.Generate_JWT(theUser);
        var refreshToken = _jwtService.Generate_RefreshToken(theUser.Id.ToString());

        _context.RefreshTokenTable.Add(refreshToken);
        await _context.SaveChangesAsync();
        
        _httpContextService.SetRefreshToken(refreshToken);

        return accessToken;
    }
}