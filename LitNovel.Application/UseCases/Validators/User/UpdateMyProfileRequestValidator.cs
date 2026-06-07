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
                .Must(x => string.IsNullOrWhiteSpace(x) || Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithMessage("Avatar must be a valid URL");

            RuleFor(x => x.Bio).MaximumLength(1000);
        }
    }
}
