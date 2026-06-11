using FluentValidation;
using LitNovel.Application.DTOs.Reading;

namespace LitNovel.Application.UseCases.Validators.Reading
{
    public class SaveReadingProgressRequestValidator : AbstractValidator<SaveReadingProgressRequestDto>
    {
        public SaveReadingProgressRequestValidator()
        {
            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100);
        }
    }
}
