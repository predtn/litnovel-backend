using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetUserWarningsUseCase : IGetUserWarningsUseCase
    {
        private readonly IUserWarningRepository _userWarningRepository;

        public GetUserWarningsUseCase(IUserWarningRepository userWarningRepository)
        {
            _userWarningRepository = userWarningRepository;
        }

        public Task<PagedResult<UserWarningResponseDto>> ExecuteAsync(int userId, int page, int size, CancellationToken ct)
        {
            var safePage = page <= 0 ? 1 : page;
            var safeSize = size <= 0 ? 20 : Math.Min(size, 100);
            return _userWarningRepository.GetByUserIdAsync(userId, safePage, safeSize, ct);
        }
    }
}
