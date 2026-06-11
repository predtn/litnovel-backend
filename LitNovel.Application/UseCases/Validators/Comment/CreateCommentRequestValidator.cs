using FluentValidation;
using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.UseCases.Validators.Comment
{
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequestDto>
    {
        public CreateCommentRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
