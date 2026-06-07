using FluentValidation;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases.Validators.Novel
{
    public class NovelListQueryValidator : AbstractValidator<NovelListQueryDto>
    {
        public NovelListQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
