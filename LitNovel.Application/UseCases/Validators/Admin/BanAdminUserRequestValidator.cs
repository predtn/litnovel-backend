using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class BanAdminUserRequestValidator : AbstractValidator<BanAdminUserRequestDto>
    {
        public BanAdminUserRequestValidator()
        {
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(1000);
        }
    }
}
