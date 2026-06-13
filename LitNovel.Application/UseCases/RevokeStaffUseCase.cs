using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class RevokeStaffUseCase : IRevokeStaffUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RevokeStaffUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StaffRoleChangeResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("User not found");

            if (user.Role == UserRole.Admin)
            {
                throw new BadRequestException("Admin users cannot have staff access revoked");
            }

            user.Role = UserRole.User;
            await _unitOfWork.SaveChangesAsync(ct);

            return new StaffRoleChangeResponseDto
            {
                UserId = user.Id,
                Role = user.Role.ToString(),
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
