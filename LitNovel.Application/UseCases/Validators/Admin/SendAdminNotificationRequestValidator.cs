using FluentValidation;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class SendAdminNotificationRequestValidator : AbstractValidator<SendAdminNotificationRequestDto>
    {
        public SendAdminNotificationRequestValidator()
        {
            RuleFor(x => x.NotificationType)
                .NotEmpty().WithMessage("Notification type is required")
                .Must(type => Enum.TryParse<NotificationType>(type, true, out _))
                .WithMessage("Invalid notification type");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Notification message is required")
                .MaximumLength(1000).WithMessage("Notification message must be at most 1000 characters");

            RuleFor(x => x.TargetAll)
                .NotNull().WithMessage("Target all is required");

            RuleFor(x => x.TargetUserId)
                .Null().WithMessage("Target user must be empty when sending to all users")
                .When(x => x.TargetAll == true);

            RuleFor(x => x.TargetUserId)
                .NotNull().WithMessage("Target user is required")
                .GreaterThan(0).WithMessage("Target user must be a valid user ID")
                .When(x => x.TargetAll == false);
        }
    }
}
