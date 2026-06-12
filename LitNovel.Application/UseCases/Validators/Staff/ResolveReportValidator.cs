using FluentValidation;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases.Validators.Staff
{
    public class ResolveReportValidator : AbstractValidator<ResolveReportRequestDto>
    {
        private static readonly string[] ValidActions = ["Resolve", "Reject"];

        public ResolveReportValidator()
        {
            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Action is required.")
                .Must(a => ValidActions.Contains(a, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Action must be 'Resolve' or 'Reject'.");

            RuleFor(x => x.ActionTaken)
                .NotEmpty().WithMessage("ActionTaken is required when resolving a report.")
                .MaximumLength(1000).WithMessage("ActionTaken must not exceed 1000 characters.")
                .When(x => x.Action != null &&
                           x.Action.Equals("Resolve", StringComparison.OrdinalIgnoreCase));

            RuleFor(x => x.ResolutionNotes)
                .MaximumLength(1000).WithMessage("ResolutionNotes must not exceed 1000 characters.")
                .When(x => x.ResolutionNotes != null);
        }
    }
}
