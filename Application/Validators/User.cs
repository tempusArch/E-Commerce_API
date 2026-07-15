using System.Data;
using FluentValidation;

namespace ECommerceAPI.Application;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto> {
    public RegisterUserDtoValidator() {
        RuleFor(x => x.Name).NotEmpty().WithMessage("User name is required");
        
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");

        RuleFor(x => x.Password).Length(8, 16).WithMessage("Password must be at least 8 characters, less than 16 characters");

        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Must match with password");
              
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
    public CreateUserCommandValidator() {
        RuleFor(x => x.RegisterUserDto).NotNull().SetValidator(new RegisterUserDtoValidator());
    }
}

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto> {
    public LoginUserDtoValidator() {   
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
              
    }
}

public class LoginUserCommandtValidator : AbstractValidator<LoginUserCommand> {
    public LoginUserCommandtValidator() {
        RuleFor(x => x.LoginUserDto).NotNull().SetValidator(new LoginUserDtoValidator());
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