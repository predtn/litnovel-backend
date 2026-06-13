using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminUserDetailUseCase
    {
        Task<AdminUserDetailResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
