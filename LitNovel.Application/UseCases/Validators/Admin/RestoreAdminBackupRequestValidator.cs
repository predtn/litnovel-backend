using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class RestoreAdminBackupRequestValidator : AbstractValidator<RestoreAdminBackupRequestDto>
    {
        public RestoreAdminBackupRequestValidator()
        {
            RuleFor(x => x.ConfirmationText)
                .Equal("RESTORE").WithMessage("Confirmation text must be 'RESTORE'");
        }
    }
}
