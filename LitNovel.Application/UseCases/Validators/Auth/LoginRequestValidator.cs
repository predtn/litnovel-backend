using FluentValidation;
using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.UseCases.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Identifier).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
