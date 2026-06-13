using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUnbanAdminUserUseCase
    {
        Task<UnbanAdminUserResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
