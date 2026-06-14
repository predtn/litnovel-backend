using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminAnnouncementUseCase
    {
        Task<AdminAnnouncementResponseDto> ExecuteAsync(int id, UpdateAdminAnnouncementRequestDto request, CancellationToken ct);
    }
}
