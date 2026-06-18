using FluentValidation;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases.Validators.Staff
{
    public class ModerateChapterValidator : AbstractValidator<ModerateChapterRequestDto>
    {
        private static readonly string[] ValidActions = ["Approve", "Reject", "Lock"];

        public ModerateChapterValidator()
        {
            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Action is required.")
                .Must(a => ValidActions.Contains(a, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Action must be 'Approve', 'Reject', or 'Lock'.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required when rejecting or locking.")
                .MaximumLength(1000).WithMessage("Reason must not exceed 1000 characters.")
                .When(x => x.Action != null &&
                           (x.Action.Equals("Reject", StringComparison.OrdinalIgnoreCase) ||
                            x.Action.Equals("Lock",   StringComparison.OrdinalIgnoreCase)));
        }
    }
}
