using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminUserUseCase : IUpdateAdminUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<UpdateAdminUserRequestDto> _validator;

        public UpdateAdminUserUseCase(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<UpdateAdminUserRequestDto> validator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<AdminUserUpdateResponseDto> ExecuteAsync(int id, UpdateAdminUserRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var user = await _userRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("User not found");

            var newRole = user.Role;
            var newStatus = user.Status;

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                newRole = Enum.Parse<UserRole>(request.Role, true);
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                newStatus = Enum.Parse<UserStatus>(request.Status, true);
            }

            await EnsureAdminCanBeChangedAsync(user.Id, user.Role, newRole, newStatus, ct);

            user.Role = newRole;
            user.Status = newStatus;
            await _unitOfWork.SaveChangesAsync(ct);

            return new AdminUserUpdateResponseDto
            {
                UserId = user.Id,
                Role = user.Role.ToString(),
                Status = user.Status.ToString(),
                UpdatedAt = user.UpdatedAt
            };
        }

        private async Task EnsureAdminCanBeChangedAsync(int targetUserId, UserRole currentRole, UserRole newRole, UserStatus newStatus, CancellationToken ct)
        {
            if (targetUserId == _currentUserService.UserId && (newRole != UserRole.Admin || newStatus == UserStatus.Banned))
            {
                throw new BadRequestException("You cannot remove your own admin access");
            }

            if (currentRole == UserRole.Admin && newRole != UserRole.Admin)
            {
                var adminCount = await _userRepository.CountByRoleAsync(UserRole.Admin, ct);
                if (adminCount <= 1)
                {
                    throw new BadRequestException("Cannot remove the last admin");
                }
            }
        }
    }
}
