using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminUserDetailUseCase : IGetAdminUserDetailUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetAdminUserDetailUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AdminUserDetailResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetAdminUserDetailAsync(id, ct);
            return user is null ? throw new NotFoundException("User not found") : user;
        }
    }
}
