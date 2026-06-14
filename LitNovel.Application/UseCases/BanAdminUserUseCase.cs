using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class BanAdminUserUseCase : IBanAdminUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<BanAdminUserRequestDto> _validator;

        public BanAdminUserUseCase(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<BanAdminUserRequestDto> validator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<BanAdminUserResponseDto> ExecuteAsync(int id, BanAdminUserRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var user = await _userRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("User not found");

            if (user.Id == _currentUserService.UserId)
            {
                throw new BadRequestException("You cannot ban your own account");
            }

            if (user.Role == UserRole.Admin)
            {
                var adminCount = await _userRepository.CountByRoleAsync(UserRole.Admin, ct);
                if (adminCount <= 1)
                {
                    throw new BadRequestException("Cannot ban the last admin");
                }
            }

            user.Status = UserStatus.Banned;
            await _unitOfWork.SaveChangesAsync(ct);

            return new BanAdminUserResponseDto
            {
                UserId = user.Id,
                Status = user.Status.ToString(),
                BannedAt = user.UpdatedAt
            };
        }
    }
}
