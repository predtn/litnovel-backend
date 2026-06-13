using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRevokeStaffUseCase
    {
        Task<StaffRoleChangeResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
