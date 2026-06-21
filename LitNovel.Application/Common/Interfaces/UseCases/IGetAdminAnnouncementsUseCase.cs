using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminAnnouncementsUseCase
    {
        Task<IReadOnlyList<AdminAnnouncementResponseDto>> ExecuteAsync(CancellationToken ct);
    }
}
