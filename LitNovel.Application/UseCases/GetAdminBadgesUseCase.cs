using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminBadgesUseCase : IGetAdminBadgesUseCase
    {
        private readonly IBadgeRepository _badgeRepository;

        public GetAdminBadgesUseCase(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        public Task<IReadOnlyList<AdminBadgeResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return _badgeRepository.GetAdminBadgesAsync(ct);
        }
    }
}
