using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetStaffUsersUseCase : IGetStaffUsersUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetStaffUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedResult<StaffUserListItemResponseDto>> ExecuteAsync(int pageNumber, int pageSize, string? searchKeyword, CancellationToken ct)
        {
            return await _userRepository.GetStaffUsersAsync(pageNumber, pageSize, searchKeyword, ct);
        }
    }
}
