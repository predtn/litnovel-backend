using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminUsersUseCase : IGetAdminUsersUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetAdminUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IQueryable<AdminUserListItemResponseDto> ExecuteQuery()
        {
            return _userRepository.QueryAdminUsers();
        }
    }
}
