using System.Data;
using FluentValidation;

namespace ECommerceAPI.Application;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto> {
    public RegisterUserDtoValidator() {
        RuleFor(x => x.Name).NotEmpty().WithMessage("User name is required");
        
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");

        RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password must be at least 8 characters");

        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Must match with password");
              
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUser.CreateUserCommand> {
    public CreateUserCommandValidator() {
        RuleFor(x => x.RegisterUserDto).SetValidator(new RegisterUserDtoValidator());
    }
}

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto> {
    public LoginUserDtoValidator() {   
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
              
    }
}

public class UserLoginCommandtValidator : AbstractValidator<UserLogin.UserLoginCommand> {
    public UserLoginCommandtValidator() {
        RuleFor(x => x.LoginUserDto).SetValidator(new LoginUserDtoValidator());
    }
}

