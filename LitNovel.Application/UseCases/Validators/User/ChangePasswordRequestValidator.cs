using FluentValidation;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases.Validators.User
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must differ from current");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
