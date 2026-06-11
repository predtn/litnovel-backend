using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Reading;

namespace LitNovel.Application.UseCases
{
    public class GetReadingHistoryUseCase : IGetReadingHistoryUseCase
    {
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetReadingHistoryUseCase(IReadingProgressRepository readingProgressRepository, ICurrentUserService currentUserService)
        {
            _readingProgressRepository = readingProgressRepository;
            _currentUserService = currentUserService;
        }

        public Task<PagedResult<ReadingProgressResponseDto>> ExecuteAsync(ReadingHistoryQueryDto query, CancellationToken ct)
        {
            return _readingProgressRepository.GetHistoryAsync(_currentUserService.UserId, query, ct);
        }
    }
}
