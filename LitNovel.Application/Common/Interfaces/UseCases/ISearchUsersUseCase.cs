using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ISearchUsersUseCase
    {
        Task<PagedResult<UserSearchResponseDto>> ExecuteAsync(UserSearchQueryDto query, CancellationToken ct);
    }
}
