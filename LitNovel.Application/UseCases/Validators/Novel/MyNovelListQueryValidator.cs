using FluentValidation;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases.Validators.Novel
{
    public class MyNovelListQueryValidator : AbstractValidator<MyNovelListQueryDto>
    {
        public MyNovelListQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
            RuleFor(x => x.Status)
                .Must(status => string.IsNullOrWhiteSpace(status) || Enum.TryParse<NovelStatus>(status, true, out _))
                .WithMessage("Invalid novel status");
            RuleFor(x => x.Sort)
                .Must(sort => string.IsNullOrWhiteSpace(sort) || sort.Equals("updatedAt", StringComparison.OrdinalIgnoreCase) || sort.Equals("createdAt", StringComparison.OrdinalIgnoreCase) || sort.Equals("viewCount", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Invalid sort field");
            RuleFor(x => x.Order)
                .Must(order => string.IsNullOrWhiteSpace(order) || order.Equals("asc", StringComparison.OrdinalIgnoreCase) || order.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Invalid sort order");
        }
    }
}
