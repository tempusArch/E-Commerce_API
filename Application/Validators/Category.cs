using System.Data;
using FluentValidation;

namespace ECommerceAPI.Application;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand> {
    public CreateCategoryCommandValidator() {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Category name can not be empty");
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand> {
    public UpdateCategoryCommandValidator() {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category name can not be empty");

        RuleFor(x => x.CategoryId).NotEmpty().GreaterThan(0).WithMessage("Category Id must be greater than 0");

    }
}

public class GetOneCategoryQueryValidator : AbstractValidator<GetOneCategoryQuery> {
    public GetOneCategoryQueryValidator() {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Category Id must be greater than 0");
    }
}

public class GetManyCategoriesQueryValidator : AbstractValidator<GetManyCategoriesQuery> {
    public GetManyCategoriesQueryValidator() {
        RuleFor(x => x.Page).NotEmpty().GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.Limit).NotEmpty().GreaterThan(0).WithMessage("Limit Id must be greater than 0");
    }
}