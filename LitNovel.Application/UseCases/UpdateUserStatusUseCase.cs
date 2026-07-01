using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class UpdateUserStatusUseCase : IUpdateUserStatusUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IModerationLogRepository _moderationLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserStatusUseCase(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IModerationLogRepository moderationLogRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _moderationLogRepository = moderationLogRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int userId, UpdateUserStatusRequestDto request, CancellationToken ct)
        {
            var targetUser = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new NotFoundException("User not found.");

            // Kểm tra phân quyền: Staff không thể thao tác lên Admin hoặc Staff khác
            if (targetUser.Role == UserRole.Admin || targetUser.Role == UserRole.Staff)
            {
                throw new ForbiddenException("Staff không có quyền thao tác trên tài khoản Admin hoặc Staff khác.");
            }

            if (!Enum.TryParse<UserStatus>(request.Status, true, out var newStatus))
            {
                throw new BadRequestException("Trạng thái không hợp lệ.");
            }

            var oldStatus = targetUser.Status;
            targetUser.Status = newStatus;

            // Ghi log
            var log = new ModerationLog
            {
                StaffId = _currentUserService.UserId,
                TargetType = "User",
                TargetId = targetUser.Id,
                Action = $"Thay đổi trạng thái từ {oldStatus} sang {newStatus}",
                Notes = request.Reason,
                PerformedAt = DateTime.UtcNow
            };
            await _moderationLogRepository.AddAsync(log, ct);

            // Thu hồi token nếu bị Ban
            if (newStatus == UserStatus.Banned)
            {
                await _refreshTokenRepository.RevokeAllForUserAsync(targetUser.Id, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
