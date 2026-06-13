using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class AwardBadgeUseCase : IAwardBadgeUseCase
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AwardBadgeUseCase(
            IBadgeRepository badgeRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _badgeRepository = badgeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AwardBadgeResponseDto> ExecuteAsync(int id, int userId, CancellationToken ct)
        {
            var badge = await _badgeRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Badge not found");

            _ = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new NotFoundException("User not found");

            if (await _badgeRepository.UserBadgeExistsAsync(id, userId, ct))
            {
                throw new ConflictException("User already has this badge");
            }

            var userBadge = new UserBadge
            {
                UserId = userId,
                BadgeId = id,
                EarnedAt = DateTime.UtcNow
            };

            await _badgeRepository.AddUserBadgeAsync(userBadge, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new AwardBadgeResponseDto
            {
                UserId = userId,
                BadgeId = id,
                Key = badge.Key,
                Name = badge.Name,
                EarnedAt = userBadge.EarnedAt
            };
        }
    }
}
