using System.Data;
using FluentValidation;

namespace ECommerceAPI.Application;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto> {
    public CreateProductDtoValidator() {
        RuleFor(cp => cp.Name).NotEmpty().WithMessage("Product name can not be empty")
            .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters");

        RuleFor(cp => cp.Description).NotEmpty().WithMessage("Product description can not be empty")
            .MaximumLength(600).WithMessage("Product description can not exceed 600 characters");

        RuleFor(cp => cp.Price).GreaterThan(0).WithMessage("Price must be greater than 0")
            .PrecisionScale(10, 2, false).WithMessage("Price have a maximum of 10 digits and 2 decimal places.");

        RuleFor(cp => cp.Quantity).GreaterThanOrEqualTo(0).WithMessage("Product quantity can not be negative");

        RuleFor(cp => cp.CategoryIdRisuto).NotEmpty().WithMessage("CategortId can not be empty");
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand> {
    public CreateProductCommandValidator() {
        RuleFor(x => x.CreateProductDto).NotNull().SetValidator(new CreateProductDtoValidator());
    }
}

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto> {
    public UpdateProductDtoValidator() {
        RuleFor(up => up.Name).NotEmpty().WithMessage("Product name can not be empty")
            .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters");

        RuleFor(up => up.Description).NotEmpty().WithMessage("Product description can not be empty")
            .MaximumLength(600).WithMessage("Product description can not exceed 600 characters");

        RuleFor(up => up.Price).GreaterThan(0).WithMessage("Price must be greater than 0")
            .PrecisionScale(10, 2, false).WithMessage("Price have a maximum of 10 digits and 2 decimal places.");
            
        RuleFor(up => up.Quantity).GreaterThanOrEqualTo(0).WithMessage("Product quantity can not be negative");

        RuleFor(up => up.CategoryIdRisuto).NotEmpty().WithMessage("CategortId can not be empty");
    }
}

public class UpdateProdcutCommandValidator : AbstractValidator<UpdateProductCommand> {
    public UpdateProdcutCommandValidator() {
        RuleFor(x => x.UpdateProductDto).NotNull().SetValidator(new UpdateProductDtoValidator());
    }
}