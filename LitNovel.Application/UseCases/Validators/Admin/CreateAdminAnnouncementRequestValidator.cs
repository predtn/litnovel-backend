using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class CreateAdminAnnouncementRequestValidator : AbstractValidator<CreateAdminAnnouncementRequestDto>
    {
        public CreateAdminAnnouncementRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Announcement title is required")
                .MaximumLength(200).WithMessage("Announcement title must be at most 200 characters");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Announcement content is required")
                .MaximumLength(2000).WithMessage("Announcement content must be at most 2000 characters");

            RuleFor(x => x.StartDate)
                .NotNull().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue && x.StartDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Active status is required");
        }
    }
}
