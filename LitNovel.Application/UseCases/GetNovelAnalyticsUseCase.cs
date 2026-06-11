using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases
{
    public class GetNovelAnalyticsUseCase : IGetNovelAnalyticsUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetNovelAnalyticsUseCase(INovelRepository novelRepository, ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
        }

        public async Task<NovelAnalyticsResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to view this novel's analytics");
            }

            return await _novelRepository.GetAnalyticsAsync(id, ct);
        }
    }
}
