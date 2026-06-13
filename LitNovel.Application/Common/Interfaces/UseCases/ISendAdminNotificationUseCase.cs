using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ISendAdminNotificationUseCase
    {
        Task<SendAdminNotificationResponseDto> ExecuteAsync(SendAdminNotificationRequestDto request, CancellationToken ct);
    }
}
