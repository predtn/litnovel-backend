using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class AdminAuditLogQueryValidator : AbstractValidator<AdminAuditLogQueryDto>
    {
        public AdminAuditLogQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
            RuleFor(x => x.ActorId)
                .GreaterThan(0)
                .When(x => x.ActorId.HasValue)
                .WithMessage("Actor must be a valid user ID");
            RuleFor(x => x.EntityType)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.EntityType));
            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("To date must be greater than or equal to from date");
        }
    }
}
