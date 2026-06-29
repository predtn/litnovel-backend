using LitNovel.Application.DTOs.Announcement;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAnnouncementsUseCase
    {
        Task<IReadOnlyList<AnnouncementResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
