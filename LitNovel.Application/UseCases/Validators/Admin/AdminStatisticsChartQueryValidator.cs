using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class AdminStatisticsChartQueryValidator : AbstractValidator<AdminStatisticsChartQueryDto>
    {
        private static readonly string[] Metrics =
        {
            "userGrowth",
            "novelGrowth",
            "chapterPublished",
            "comments",
            "ratings",
            "favorites",
            "reports"
        };

        private static readonly string[] Granularities = { "day", "month" };

        public AdminStatisticsChartQueryValidator()
        {
            RuleFor(x => x.Metric)
                .NotEmpty()
                .WithMessage("Metric is required")
                .Must(metric => Metrics.Contains(metric))
                .WithMessage("Invalid statistics metric");

            RuleFor(x => x.From)
                .NotNull()
                .WithMessage("From date is required");

            RuleFor(x => x.To)
                .NotNull()
                .WithMessage("To date is required");

            RuleFor(x => x.Granularity)
                .NotEmpty()
                .WithMessage("Granularity is required")
                .Must(granularity => Granularities.Contains(granularity.ToLowerInvariant()))
                .WithMessage("Invalid granularity");

            RuleFor(x => x.To)
                .GreaterThanOrEqualTo(x => x.From)
                .When(x => x.From.HasValue && x.To.HasValue)
                .WithMessage("To date must be greater than or equal to from date");

            RuleFor(x => x)
                .Must(x => !x.From.HasValue || !x.To.HasValue || (x.To.Value.Date - x.From.Value.Date).TotalDays <= 366)
                .WithMessage("Date range cannot exceed 366 days");
        }
    }
}
