using FluentValidation;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminUserRequestValidator : AbstractValidator<UpdateAdminUserRequestDto>
    {
        public UpdateAdminUserRequestValidator()
        {
            RuleFor(x => x.Role)
                .Must(role => string.IsNullOrWhiteSpace(role) || Enum.TryParse<UserRole>(role, true, out _))
                .WithMessage("Invalid user role");

            RuleFor(x => x.Status)
                .Must(BeAssignableStatus)
                .WithMessage("Invalid user status. Admin user management supports only Offline or Online.");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Role) || !string.IsNullOrWhiteSpace(x.Status))
                .WithMessage("Role or status is required");
        }

        private static bool BeAssignableStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return true;
            }

            return Enum.TryParse<UserStatus>(status, true, out var parsedStatus)
                && parsedStatus is UserStatus.Offline or UserStatus.Online;
        }
    }
}
