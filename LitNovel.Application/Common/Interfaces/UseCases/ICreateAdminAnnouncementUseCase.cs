using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateAdminAnnouncementUseCase
    {
        Task<AdminAnnouncementSummaryResponseDto> ExecuteAsync(CreateAdminAnnouncementRequestDto request, CancellationToken ct);
    }
}
