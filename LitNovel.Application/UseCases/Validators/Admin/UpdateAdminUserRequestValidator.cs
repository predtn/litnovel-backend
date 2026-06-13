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
                .Must(status => string.IsNullOrWhiteSpace(status) || Enum.TryParse<UserStatus>(status, true, out _))
                .WithMessage("Invalid user status");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Role) || !string.IsNullOrWhiteSpace(x.Status))
                .WithMessage("Role or status is required");
        }
    }
}
