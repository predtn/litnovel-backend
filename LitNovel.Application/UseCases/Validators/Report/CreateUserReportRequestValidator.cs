using FluentValidation;
using LitNovel.Application.DTOs.Report;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Report
{
    public class CreateUserReportRequestValidator : AbstractValidator<CreateUserReportRequestDto>
    {
        public CreateUserReportRequestValidator()
        {
            RuleFor(x => x.TargetUserId).GreaterThan(0);
            RuleFor(x => x.ReportType).NotEmpty().Must(x => Enum.TryParse<ReportType>(x, true, out _)).WithMessage("Invalid report type");
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
