using FluentValidation;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminChapterStatusRequestValidator : AbstractValidator<UpdateAdminChapterStatusRequestDto>
    {
        public UpdateAdminChapterStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(status => Enum.TryParse<ChapterStatus>(status, true, out _))
                .WithMessage("Invalid chapter status");

            RuleFor(x => x.Reason)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Reason));
        }
    }
}
