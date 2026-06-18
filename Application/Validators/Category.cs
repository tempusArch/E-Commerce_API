using System.Data;
using FluentValidation;

namespace ECommerceAPI.Application;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategory.CreateCategoryCommand> {
    public CreateCategoryCommandValidator() {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category name can not be empty");
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategory.UpdateCategoryCommand> {
    public UpdateCategoryCommandValidator() {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category name can not be empty");
    }
}