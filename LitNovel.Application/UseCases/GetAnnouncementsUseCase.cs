using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Announcement;

namespace LitNovel.Application.UseCases
{
    public class GetAnnouncementsUseCase : IGetAnnouncementsUseCase
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public GetAnnouncementsUseCase(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public Task<IReadOnlyList<AnnouncementResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return _announcementRepository.GetActiveAsync(DateTime.UtcNow, ct);
        }
    }
}
