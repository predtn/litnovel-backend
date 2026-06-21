using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetPendingChaptersUseCase : IGetPendingChaptersUseCase
    {
        private readonly IChapterRepository _chapterRepository;

        public GetPendingChaptersUseCase(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        public Task<PagedResult<PendingChapterListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct)
        {
            var safePage = page <= 0 ? 1 : page;
            var safeSize = size <= 0 ? 20 : Math.Min(size, 100);
            return _chapterRepository.GetPendingAsync(safePage, safeSize, ct);
        }
    }
}
