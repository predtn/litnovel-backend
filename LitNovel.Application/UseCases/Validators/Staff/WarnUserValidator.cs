using FluentValidation;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases.Validators.Staff
{
    public class WarnUserValidator : AbstractValidator<WarnUserRequestDto>
    {
        private static readonly string[] ValidSeverities = ["Minor", "Major"];

        public WarnUserValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Warning reason is required.")
                .MaximumLength(1000).WithMessage("Reason must not exceed 1000 characters.");

            RuleFor(x => x.Severity)
                .NotEmpty().WithMessage("Severity is required.")
                .Must(s => ValidSeverities.Contains(s))
                .WithMessage("Severity must be 'Minor' or 'Major'.");
        }
    }
}
