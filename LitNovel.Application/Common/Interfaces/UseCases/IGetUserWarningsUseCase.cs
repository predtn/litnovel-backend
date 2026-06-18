using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetUserWarningsUseCase
    {
        Task<PagedResult<UserWarningResponseDto>> ExecuteAsync(int userId, int page, int size, CancellationToken ct);
    }
}
