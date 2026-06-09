using FluentValidation;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Chapter
{
    public class ChapterListQueryValidator : AbstractValidator<ChapterListQueryDto>
    {
        private static readonly string[] SortFields = { "chapterNumber", "createdAt" };
        private static readonly string[] Orders = { "asc", "desc" };

        public ChapterListQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
            RuleFor(x => x.Status)
                .Must(status => string.IsNullOrWhiteSpace(status) || Enum.TryParse<ChapterStatus>(status, true, out _))
                .WithMessage("Invalid chapter status");
            RuleFor(x => x.Sort)
                .Must(x => string.IsNullOrWhiteSpace(x) || SortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Invalid sort field");
            RuleFor(x => x.Order)
                .Must(x => string.IsNullOrWhiteSpace(x) || Orders.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Invalid order");
        }
    }
}
