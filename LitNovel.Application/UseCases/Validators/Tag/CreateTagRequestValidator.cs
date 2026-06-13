using FluentValidation;
using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.UseCases.Validators.Tag
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequestDto>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag name is required")
                .MaximumLength(100).WithMessage("Tag name must be at most 100 characters");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .MaximumLength(120).WithMessage("Slug must be at most 120 characters")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be URL-safe");
        }
    }
}
