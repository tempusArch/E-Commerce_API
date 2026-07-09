using ECommerceAPI.Domain;
using FluentValidation;

namespace ECommerceAPI.Application;

public class CartItemValidator : AbstractValidator<CartItem> {
    public CartItemValidator() {
        RuleFor(ci => ci.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be empty");

        RuleFor(ci => ci.CartId).NotEmpty().GreaterThan(0).WithMessage("Cart Id can not be empty");

        RuleFor(ci => ci.Quantity).NotEmpty().GreaterThan(0).WithMessage("Quantity can not be empty");
    }
}

public class AddIntoCartItemCommandValidator : AbstractValidator<AddIntoCartItemCommand> {
    public AddIntoCartItemCommandValidator() {
        RuleFor(x => x.CartItem).SetValidator(new CartItemValidator());
    }
}

public class UpdateSingleCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand> {
    public UpdateSingleCartItemCommandValidator() {
        RuleFor(x => x.CartItem).SetValidator(new CartItemValidator());
    }
}

public class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand> {
    public DeleteCartItemCommandValidator() {
        RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be 0 or negative");

        RuleFor(x => x.CartId).NotEmpty().GreaterThan(0).WithMessage("Cart Id can not be 0 or negative");

    }
}

public class GetAllCartItemsQueryValidator : AbstractValidator<GetAllCartItemsQuery> {
    GetAllCartItemsQueryValidator() {
        RuleFor(x => x.CartId).NotEmpty().GreaterThan(0).WithMessage("Cart Id can not be 0 or negative");
    }
}

public class GetSingleCartItemQueryValidator : AbstractValidator<GetOneCartItemQuery> {
    public GetSingleCartItemQueryValidator() {
        RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0).WithMessage("Product Id can not be 0 or negative");
        
        RuleFor(x => x.CartId).NotEmpty().GreaterThan(0).WithMessage("Cart Id can not be 0 or negative");
    }
}