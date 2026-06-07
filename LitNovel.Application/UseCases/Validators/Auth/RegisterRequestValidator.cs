using FluentValidation;
using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.UseCases.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(3, 50)
                .WithMessage("Username must be 3-50 characters")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("Username may contain only letters, numbers, and underscores");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format")
                .MaximumLength(256);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit");
        }
    }
}
