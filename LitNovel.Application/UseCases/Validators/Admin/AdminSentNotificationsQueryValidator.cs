using FluentValidation;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class AdminSentNotificationsQueryValidator : AbstractValidator<AdminSentNotificationsQueryDto>
    {
        public AdminSentNotificationsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
            RuleFor(x => x.NotificationType)
                .Must(type => string.IsNullOrWhiteSpace(type) || Enum.TryParse<NotificationType>(type, true, out _))
                .WithMessage("Invalid notification type");
            RuleFor(x => x.TargetUserId)
                .GreaterThan(0)
                .When(x => x.TargetUserId.HasValue)
                .WithMessage("Target user must be a valid user ID");
        }
    }
}
