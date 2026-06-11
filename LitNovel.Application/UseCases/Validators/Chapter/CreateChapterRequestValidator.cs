using FluentValidation;
using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.UseCases.Validators.Chapter
{
    public class CreateChapterRequestValidator : AbstractValidator<CreateChapterRequestDto>
    {
        public CreateChapterRequestValidator()
        {
            RuleFor(x => x.ChapterNumber).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.ReleaseDate)
                .Must(x => !x.HasValue || x.Value > DateTime.UtcNow)
                .WithMessage("Release date must be a future datetime");
        }
    }
}
