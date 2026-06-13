using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateAdminBadgeUseCase : ICreateAdminBadgeUseCase
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateAdminBadgeRequestDto> _validator;

        public CreateAdminBadgeUseCase(
            IBadgeRepository badgeRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateAdminBadgeRequestDto> validator)
        {
            _badgeRepository = badgeRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminBadgeResponseDto> ExecuteAsync(CreateAdminBadgeRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var key = request.Key.Trim();
            if (await _badgeRepository.KeyExistsAsync(key, excludingId: null, ct))
            {
                throw new ConflictException("Badge key already exists");
            }

            var badge = new Badge
            {
                Key = key,
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Icon = NormalizeOptional(request.Icon),
                Color = NormalizeOptional(request.Color)
            };

            await _badgeRepository.AddAsync(badge, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Map(badge);
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static AdminBadgeResponseDto Map(Badge badge)
        {
            return new AdminBadgeResponseDto
            {
                Id = badge.Id,
                Key = badge.Key,
                Name = badge.Name,
                Description = badge.Description,
                Icon = badge.Icon,
                Color = badge.Color,
                AwardedCount = badge.UserBadges.Count
            };
        }
    }
}
