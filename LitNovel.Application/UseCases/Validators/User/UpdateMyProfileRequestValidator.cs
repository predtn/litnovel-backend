using FluentValidation;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases.Validators.User
{
    public class UpdateMyProfileRequestValidator : AbstractValidator<UpdateMyProfileRequestDto>
    {
        public UpdateMyProfileRequestValidator()
        {
            RuleFor(x => x.Avatar)
                .MaximumLength(512)
                .Must(BeValidAssetReference)
                .WithMessage("Avatar must be a valid URL or local upload path");

            RuleFor(x => x.Bio).MaximumLength(1000);
        }

        private static bool BeValidAssetReference(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            if (value.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)) return true;

            return Uri.TryCreate(value, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
