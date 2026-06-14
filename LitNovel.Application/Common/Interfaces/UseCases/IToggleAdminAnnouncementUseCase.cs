using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IToggleAdminAnnouncementUseCase
    {
        Task<AdminAnnouncementResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
