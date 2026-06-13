using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminStatisticsUseCase
    {
        Task<AdminStatisticsResponseDto> ExecuteAsync(CancellationToken ct);
    }
}
