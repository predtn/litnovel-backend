using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetModerationHistoryUseCase : IGetModerationHistoryUseCase
    {
        private readonly IModerationLogRepository _moderationLogRepository;

        public GetModerationHistoryUseCase(IModerationLogRepository moderationLogRepository)
        {
            _moderationLogRepository = moderationLogRepository;
        }

        public Task<PagedResult<ModerationHistoryItemResponseDto>> ExecuteAsync(
            int? staffId,
            string? actionType,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int size,
            CancellationToken ct)
        {
            var safePage = page <= 0 ? 1 : page;
            var safeSize = size <= 0 ? 20 : Math.Min(size, 100);

            return _moderationLogRepository.GetListAsync(staffId, actionType, fromDate, toDate, safePage, safeSize, ct);
        }
    }
}
