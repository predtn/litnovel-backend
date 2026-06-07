using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases
{
    public class GetMyProfileUseCase : IGetMyProfileUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyProfileUseCase(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<MyProfileResponseDto> ExecuteAsync(CancellationToken ct)
        {
            var user = await _userRepository.GetByIdWithProfileAsync(_currentUserService.UserId, ct);
            return user is null ? throw new NotFoundException("User not found") : UserProfileMapper.ToMyProfile(user);
        }
    }
}
