using ECommerceAPI.Domain;
using FluentValidation;

namespace ECommerceAPI.Application;

public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand> {
    public CreateCartItemCommandValidator() {
        RuleFor(ci => ci.UserId).NotEmpty().GreaterThan(0).WithMessage("User Id can not be empty");

        RuleFor(ci => ci.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be empty");

        RuleFor(ci => ci.Quantity).NotEmpty().GreaterThan(0).WithMessage("Quantity can not be empty");
    }
}

public class UpdateSingleCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand> {
    public UpdateSingleCartItemCommandValidator() {
        RuleFor(ci => ci.UserId).NotEmpty().GreaterThan(0).WithMessage("User Id can not be empty");

        RuleFor(ci => ci.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be empty");

        RuleFor(ci => ci.Quantity).NotEmpty().GreaterThan(0).WithMessage("Quantity can not be empty");
    }
}

public class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand> {
    public DeleteCartItemCommandValidator() {
        RuleFor(ci => ci.UserId).NotEmpty().GreaterThan(0).WithMessage("User Id can not be empty");

        RuleFor(ci => ci.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be empty");

    }
}

public class GetAllCartItemsQueryValidator : AbstractValidator<GetAllCartItemsQuery> {
    public GetAllCartItemsQueryValidator() {
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0).WithMessage("User Id can not be 0 or negative");
    }
}

public class GetSingleCartItemQueryValidator : AbstractValidator<GetOneCartItemQuery> {
    public GetSingleCartItemQueryValidator() {
        RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be 0 or negative");
        
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0).WithMessage("User Id can not be 0 or negative");
    }
}