using FluentValidation;
using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.UseCases.Validators.Tag
{
    public class UpdateTagRequestValidator : AbstractValidator<UpdateTagRequestDto>
    {
        public UpdateTagRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag name is required")
                .MaximumLength(100).WithMessage("Tag name must be at most 100 characters")
                .When(x => x.Name is not null);

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .MaximumLength(120).WithMessage("Slug must be at most 120 characters")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be URL-safe")
                .When(x => x.Slug is not null);

            RuleFor(x => x)
                .Must(x => x.Name is not null || x.Slug is not null)
                .WithMessage("At least one tag field is required");
        }
    }
}
