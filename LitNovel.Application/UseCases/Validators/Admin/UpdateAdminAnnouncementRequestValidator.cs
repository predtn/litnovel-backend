using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminAnnouncementRequestValidator : AbstractValidator<UpdateAdminAnnouncementRequestDto>
    {
        public UpdateAdminAnnouncementRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Announcement title is required")
                .MaximumLength(200).WithMessage("Announcement title must be at most 200 characters")
                .When(x => x.Title is not null);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Announcement content is required")
                .MaximumLength(2000).WithMessage("Announcement content must be at most 2000 characters")
                .When(x => x.Content is not null);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue && x.StartDate.HasValue)
                .WithMessage("End date must be after start date");
        }
    }
}
