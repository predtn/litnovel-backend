using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class DeleteAdminUserUseCase : IDeleteAdminUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAdminUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("User not found");

            if (user.Id == _currentUserService.UserId)
            {
                throw new BadRequestException("You cannot delete your own account");
            }

            if (user.Role == UserRole.Admin)
            {
                var adminCount = await _userRepository.CountByRoleAsync(UserRole.Admin, ct);
                if (adminCount <= 1)
                {
                    throw new BadRequestException("Cannot delete the last admin");
                }
            }

            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
