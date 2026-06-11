using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases
{
    public class SearchUsersUseCase : ISearchUsersUseCase
    {
        private readonly IUserRepository _userRepository;

        public SearchUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<PagedResult<UserSearchResponseDto>> ExecuteAsync(UserSearchQueryDto query, CancellationToken ct)
        {
            return _userRepository.SearchAsync(query, ct);
        }
    }
}
