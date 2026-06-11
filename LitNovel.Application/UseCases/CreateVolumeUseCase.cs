using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Volume;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateVolumeUseCase : ICreateVolumeUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IVolumeRepository _volumeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateVolumeRequestDto> _validator;

        public CreateVolumeUseCase(
            INovelRepository novelRepository,
            IVolumeRepository volumeRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<CreateVolumeRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _volumeRepository = volumeRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<VolumeResponseDto> ExecuteAsync(int novelId, CreateVolumeRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (novelId <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(novelId, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (await _volumeRepository.VolumeNumberExistsAsync(novelId, request.VolumeNumber, null, ct))
            {
                throw new ConflictException("Volume number already exists in this novel");
            }

            var volume = new Volume
            {
                NovelId = novelId,
                VolumeNumber = request.VolumeNumber,
                Title = request.Title.Trim()
            };

            novel.TotalVolumes++;
            await _volumeRepository.AddAsync(volume, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new VolumeResponseDto
            {
                Id = volume.Id,
                VolumeNumber = volume.VolumeNumber,
                Title = volume.Title,
                ChapterCount = 0,
                CreatedAt = volume.CreatedAt
            };
        }
    }
}
