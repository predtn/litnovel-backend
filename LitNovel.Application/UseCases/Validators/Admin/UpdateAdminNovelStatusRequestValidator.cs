using FluentValidation;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminNovelStatusRequestValidator : AbstractValidator<UpdateAdminNovelStatusRequestDto>
    {
        private static readonly HashSet<NovelStatus> AllowedStatuses =
        [
            NovelStatus.Ongoing,
            NovelStatus.Ended,
            NovelStatus.Hiatus,
            NovelStatus.Dropped,
            NovelStatus.Canceled
        ];

        public UpdateAdminNovelStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(BeAllowedNovelStatus)
                .WithMessage("Invalid novel status");

            RuleFor(x => x.Reason)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Reason));
        }

        private static bool BeAllowedNovelStatus(string status)
        {
            return Enum.TryParse<NovelStatus>(status, true, out var parsedStatus)
                && AllowedStatuses.Contains(parsedStatus);
        }
    }
}
