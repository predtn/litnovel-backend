using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminSentNotificationsUseCase
    {
        Task<PagedResult<AdminSentNotificationResponseDto>> ExecuteAsync(AdminSentNotificationsQueryDto query, CancellationToken ct);
    }
}
