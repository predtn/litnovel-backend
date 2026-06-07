using FluentValidation;
using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.UseCases.Validators.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
