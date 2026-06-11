using FluentValidation;
using LitNovel.Application.DTOs.Review;

namespace LitNovel.Application.UseCases.Validators.Review
{
    public class UpdateNovelReviewRequestValidator : AbstractValidator<UpdateNovelReviewRequestDto>
    {
        public UpdateNovelReviewRequestValidator()
        {
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Review).MaximumLength(3000);
        }
    }
}
