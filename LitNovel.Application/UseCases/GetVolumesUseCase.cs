using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Volume;

namespace LitNovel.Application.UseCases
{
    public class GetVolumesUseCase : IGetVolumesUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IVolumeRepository _volumeRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetVolumesUseCase(
            INovelRepository novelRepository,
            IVolumeRepository volumeRepository,
            ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _volumeRepository = volumeRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<VolumeResponseDto>> ExecuteAsync(int novelId, CancellationToken ct)
        {
            if (novelId <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdWithDetailsAsync(novelId, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            var volumes = await _volumeRepository.GetByNovelIdAsync(novelId, ct);
            return volumes.Select(Map).ToList();
        }

        private static VolumeResponseDto Map(Domain.Entities.Volume volume)
        {
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
