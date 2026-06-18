using FluentValidation;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class AdminReportsQueryValidator : AbstractValidator<AdminReportsQueryDto>
    {
        private static readonly string[] ReportTypes = { "novel", "user" };

        public AdminReportsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
            RuleFor(x => x.Type)
                .Must(type => string.IsNullOrWhiteSpace(type) || ReportTypes.Contains(type.ToLowerInvariant()))
                .WithMessage("Invalid report target type");
            RuleFor(x => x.Status)
                .Must(status => string.IsNullOrWhiteSpace(status) || Enum.TryParse<ReportStatus>(status, true, out _))
                .WithMessage("Invalid report status");
            RuleFor(x => x.ProcessedById)
                .GreaterThan(0)
                .When(x => x.ProcessedById.HasValue)
                .WithMessage("Processed by user must be a valid user ID");
            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("To date must be greater than or equal to from date");
        }
    }
}
