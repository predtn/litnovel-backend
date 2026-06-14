using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IGetAdminUsersUseCase
    {
        IQueryable<AdminUserListItemResponseDto> ExecuteQuery();
    }
}
