using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Chapter;

namespace LitNovel.Application.UseCases
{
    public class GetChaptersUseCase : IGetChaptersUseCase
    {
        private readonly IVolumeRepository _volumeRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<ChapterListQueryDto> _validator;

        public GetChaptersUseCase(
            IVolumeRepository volumeRepository,
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IValidator<ChapterListQueryDto> validator)
        {
            _volumeRepository = volumeRepository;
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<PagedResult<ChapterListItemResponseDto>> ExecuteAsync(int volumeId, ChapterListQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            if (volumeId <= 0)
            {
                throw new BadRequestException("Invalid volume id");
            }

            var volume = await _volumeRepository.GetByIdForUpdateAsync(volumeId, ct);
            if (volume == null)
            {
                throw new NotFoundException("Volume not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            return await _chapterRepository.GetByVolumeIdAsync(volumeId, query, ct);
        }

        public async Task<IQueryable<ChapterListItemResponseDto>> ExecuteQueryAsync(int volumeId, CancellationToken ct)
        {
            await EnsureCanManageVolumeAsync(volumeId, ct);
            return _chapterRepository.QueryByVolumeId(volumeId);
        }

        private async Task EnsureCanManageVolumeAsync(int volumeId, CancellationToken ct)
        {
            if (volumeId <= 0)
            {
                throw new BadRequestException("Invalid volume id");
            }

            var volume = await _volumeRepository.GetByIdForUpdateAsync(volumeId, ct);
            if (volume == null)
            {
                throw new NotFoundException("Volume not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }
        }
    }
}
