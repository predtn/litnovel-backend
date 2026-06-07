using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.UseCases
{
    public class UpdateVolumeUseCase : IUpdateVolumeUseCase
    {
        private readonly IVolumeRepository _volumeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateVolumeRequestDto> _validator;

        public UpdateVolumeUseCase(
            IVolumeRepository volumeRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateVolumeRequestDto> validator)
        {
            _volumeRepository = volumeRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<VolumeResponseDto> ExecuteAsync(int id, UpdateVolumeRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (id <= 0)
            {
                throw new BadRequestException("Invalid volume id");
            }

            var volume = await _volumeRepository.GetByIdForUpdateAsync(id, ct);
            if (volume == null)
            {
                throw new NotFoundException("Volume not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (await _volumeRepository.VolumeNumberExistsAsync(volume.NovelId, request.VolumeNumber, id, ct))
            {
                throw new ConflictException("Volume number already exists in this novel");
            }

            volume.VolumeNumber = request.VolumeNumber;
            volume.Title = request.Title.Trim();
            await _unitOfWork.SaveChangesAsync(ct);

            return new VolumeResponseDto
            {
                Id = volume.Id,
                VolumeNumber = volume.VolumeNumber,
                Title = volume.Title,
                ChapterCount = volume.Chapters.Count,
                CreatedAt = volume.CreatedAt
            };
        }
    }
}
