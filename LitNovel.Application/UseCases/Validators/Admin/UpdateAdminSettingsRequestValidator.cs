using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminSettingsRequestValidator : AbstractValidator<UpdateAdminSettingsRequestDto>
    {
        public UpdateAdminSettingsRequestValidator()
        {
            RuleFor(x => x.General).NotNull().WithMessage("General settings are required");
            RuleFor(x => x.Content).NotNull().WithMessage("Content settings are required");
            RuleFor(x => x.Moderation).NotNull().WithMessage("Moderation settings are required");

            When(x => x.General != null, () =>
            {
                RuleFor(x => x.General!.SiteName)
                    .NotEmpty().WithMessage("Site name is required")
                    .MaximumLength(100).WithMessage("Site name must be at most 100 characters");

                RuleFor(x => x.General!.Tagline)
                    .NotNull().WithMessage("Tagline is required")
                    .MaximumLength(200).WithMessage("Tagline must be at most 200 characters");

                RuleFor(x => x.General!.MaintenanceMode)
                    .NotNull().WithMessage("Maintenance mode is required");
            });

            When(x => x.Content != null, () =>
            {
                RuleFor(x => x.Content!.MaxNovelDescriptionLength)
                    .NotNull().WithMessage("Max novel description length is required")
                    .InclusiveBetween(100, 50000).WithMessage("Max novel description length must be between 100 and 50000");

                RuleFor(x => x.Content!.MaxChapterLength)
                    .NotNull().WithMessage("Max chapter length is required")
                    .InclusiveBetween(1000, 200000).WithMessage("Max chapter length must be between 1000 and 200000");

                RuleFor(x => x.Content!.MaxTagsPerNovel)
                    .NotNull().WithMessage("Max tags per novel is required")
                    .InclusiveBetween(1, 50).WithMessage("Max tags per novel must be between 1 and 50");
            });

            When(x => x.Moderation != null, () =>
            {
                RuleFor(x => x.Moderation!.ReviewSLAHours)
                    .NotNull().WithMessage("Review SLA hours is required")
                    .InclusiveBetween(1, 720).WithMessage("Review SLA hours must be between 1 and 720");

                RuleFor(x => x.Moderation!.AutoFlagKeywords)
                    .NotNull().WithMessage("Auto flag keywords are required")
                    .Must(keywords => keywords == null || keywords.Count <= 100)
                    .WithMessage("Auto flag keywords must contain at most 100 items");

                RuleForEach(x => x.Moderation!.AutoFlagKeywords)
                    .NotEmpty().WithMessage("Auto flag keyword cannot be empty")
                    .MaximumLength(100).WithMessage("Auto flag keyword must be at most 100 characters");
            });
        }
    }
}
