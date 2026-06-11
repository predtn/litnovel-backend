using FluentValidation;
using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.UseCases.Validators.Volume
{
    public class UpdateVolumeRequestValidator : AbstractValidator<UpdateVolumeRequestDto>
    {
        public UpdateVolumeRequestValidator()
        {
            RuleFor(x => x.VolumeNumber).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        }
    }
}
