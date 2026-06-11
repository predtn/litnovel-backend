using FluentValidation;
using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.UseCases.Validators.Volume
{
    public class CreateVolumeRequestValidator : AbstractValidator<CreateVolumeRequestDto>
    {
        public CreateVolumeRequestValidator()
        {
            RuleFor(x => x.VolumeNumber).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        }
    }
}
