using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetPendingNovelsUseCase : IGetPendingNovelsUseCase
    {
        private readonly INovelRepository _novelRepository;

        public GetPendingNovelsUseCase(INovelRepository novelRepository)
        {
            _novelRepository = novelRepository;
        }

        public Task<PagedResult<PendingNovelListItemResponseDto>> ExecuteAsync(int page, int size, CancellationToken ct)
        {
            var safePage = page <= 0 ? 1 : page;
            var safeSize = size <= 0 ? 20 : Math.Min(size, 100);
            return _novelRepository.GetPendingAsync(safePage, safeSize, ct);
        }
    }
}
