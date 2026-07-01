using FluentValidation;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Novel
{
    public class UpdateNovelLifecycleStatusRequestValidator : AbstractValidator<UpdateNovelLifecycleStatusRequestDto>
    {
        public UpdateNovelLifecycleStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(BeLifecycleStatus)
                .WithMessage("Lifecycle status must be one of: Ongoing, Ended, Hiatus, Dropped");
        }

        private static bool BeLifecycleStatus(string? status)
        {
            return !string.IsNullOrWhiteSpace(status)
                && Enum.TryParse<NovelStatus>(status.Trim(), true, out var parsed)
                && parsed is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped;
        }
    }
}
