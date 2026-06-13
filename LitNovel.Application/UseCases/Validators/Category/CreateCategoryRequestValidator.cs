using FluentValidation;
using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.UseCases.Validators.Category
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequestDto>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(100).WithMessage("Category name must be at most 100 characters");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .MaximumLength(120).WithMessage("Slug must be at most 120 characters")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be URL-safe");
        }
    }
}
