using FluentValidation;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases.Validators.Admin
{
    public class UpdateAdminNovelAuthorRequestValidator : AbstractValidator<UpdateAdminNovelAuthorRequestDto>
    {
        public UpdateAdminNovelAuthorRequestValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("Invalid author id");

            RuleFor(x => x.Reason)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Reason));
        }
    }
}
