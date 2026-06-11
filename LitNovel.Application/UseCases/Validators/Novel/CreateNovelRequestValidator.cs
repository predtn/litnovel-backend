using FluentValidation;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases.Validators.Novel
{
    public class CreateNovelRequestValidator : AbstractValidator<CreateNovelRequestDto>
    {
        public CreateNovelRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(5000);
            RuleFor(x => x.CoverImage).MaximumLength(512).Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.CoverImage)).WithMessage("Cover image must be a valid URL");
            RuleFor(x => x.CategoryId)
                .NotNull()
                .WithMessage("Category is required")
                .GreaterThan(0)
                .WithMessage("Category id must be greater than 0");
            RuleFor(x => x.TagIds)
                .NotNull()
                .WithMessage("At least one tag is required")
                .NotEmpty()
                .WithMessage("At least one tag is required");
            RuleFor(x => x.TagIds)
                .Must(tagIds => tagIds == null || tagIds.Count <= 10)
                .WithMessage("A novel can have at most 10 tags");
            RuleFor(x => x.TagIds)
                .Must(tagIds => tagIds == null || tagIds.Distinct().Count() == tagIds.Count)
                .WithMessage("Duplicate tag ids are not allowed");
            RuleForEach(x => x.TagIds)
                .GreaterThan(0)
                .WithMessage("Tag id must be greater than 0");
        }

        private static bool BeValidUrl(string? value)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
