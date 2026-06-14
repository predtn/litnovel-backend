using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class UnbanAdminUserUseCase : IUnbanAdminUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnbanAdminUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnbanAdminUserResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("User not found");

            user.Status = UserStatus.Offline;
            await _unitOfWork.SaveChangesAsync(ct);

            return new UnbanAdminUserResponseDto
            {
                UserId = user.Id,
                Status = user.Status.ToString(),
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
