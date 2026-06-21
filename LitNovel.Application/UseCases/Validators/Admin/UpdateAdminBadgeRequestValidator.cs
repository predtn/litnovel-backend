using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminBadgeRequestValidator : AbstractValidator<UpdateAdminBadgeRequestDto>
    {
        public UpdateAdminBadgeRequestValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Badge key is required")
                .MaximumLength(100).WithMessage("Badge key must be at most 100 characters")
                .Matches("^[a-z0-9_]+$").WithMessage("Badge key may only contain lowercase letters, numbers, and underscores")
                .When(x => x.Key is not null);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Badge name is required")
                .MaximumLength(100).WithMessage("Badge name must be at most 100 characters")
                .When(x => x.Name is not null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Badge description is required")
                .MaximumLength(500).WithMessage("Badge description must be at most 500 characters")
                .When(x => x.Description is not null);

            RuleFor(x => x.Icon)
                .MaximumLength(512).WithMessage("Badge icon must be at most 512 characters")
                .Must(BeValidUrl).WithMessage("Badge icon must be a valid URL")
                .When(x => !string.IsNullOrWhiteSpace(x.Icon));

            RuleFor(x => x.Color)
                .Matches("^#(?:[0-9a-fA-F]{3}|[0-9a-fA-F]{6})$")
                .WithMessage("Badge color must be a valid hex color")
                .When(x => !string.IsNullOrWhiteSpace(x.Color));

            RuleFor(x => x)
                .Must(x => x.Key is not null || x.Name is not null || x.Description is not null || x.Icon is not null || x.Color is not null)
                .WithMessage("At least one badge field is required");
        }

        private static bool BeValidUrl(string? value)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
