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

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
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

public class UserLoginCommandtValidator : AbstractValidator<UserLoginCommand> {
    public UserLoginCommandtValidator() {
        RuleFor(x => x.LoginUserDto).SetValidator(new LoginUserDtoValidator());
    }
}

public class GetOneUserQueryValidator : AbstractValidator<GetOneUserQuery> {
    public GetOneUserQueryValidator() {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("User Id must be greater than 0");
    }
}

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery> {
    public GetAllUsersQueryValidator() {  
        RuleFor(x => x.Page).NotEmpty().GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.Limit).NotEmpty().GreaterThan(0).WithMessage("Limit Id must be greater than 0");


    }
}