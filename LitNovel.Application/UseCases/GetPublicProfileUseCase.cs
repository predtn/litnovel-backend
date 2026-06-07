using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases
{
    public class GetPublicProfileUseCase : IGetPublicProfileUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetPublicProfileUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PublicProfileResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdWithProfileAsync(id, ct);
            return user is null ? throw new NotFoundException("User not found") : UserProfileMapper.ToPublicProfile(user);
        }
    }
}
