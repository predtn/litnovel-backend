using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminAnnouncementsUseCase : IGetAdminAnnouncementsUseCase
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public GetAdminAnnouncementsUseCase(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public Task<IReadOnlyList<AdminAnnouncementResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return _announcementRepository.GetAllAsync(ct);
        }
    }
}
