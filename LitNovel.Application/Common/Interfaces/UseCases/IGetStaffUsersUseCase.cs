using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetStaffUsersUseCase
    {
        Task<PagedResult<StaffUserListItemResponseDto>> ExecuteAsync(int pageNumber, int pageSize, string? searchKeyword, CancellationToken ct);
    }
}
