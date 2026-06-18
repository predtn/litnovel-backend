using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminBadgeUseCase : IUpdateAdminBadgeUseCase
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAdminBadgeRequestDto> _validator;

        public UpdateAdminBadgeUseCase(
            IBadgeRepository badgeRepository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateAdminBadgeRequestDto> validator)
        {
            _badgeRepository = badgeRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminBadgeResponseDto> ExecuteAsync(int id, UpdateAdminBadgeRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var badge = await _badgeRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Badge not found");

            if (request.Key is not null)
            {
                var key = request.Key.Trim();
                if (await _badgeRepository.KeyExistsAsync(key, id, ct))
                {
                    throw new ConflictException("Badge key already exists");
                }

                badge.Key = key;
            }

            if (request.Name is not null)
            {
                badge.Name = request.Name.Trim();
            }

            if (request.Description is not null)
            {
                badge.Description = request.Description.Trim();
            }

            if (request.Icon is not null)
            {
                badge.Icon = NormalizeOptional(request.Icon);
            }

            if (request.Color is not null)
            {
                badge.Color = NormalizeOptional(request.Color);
            }

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
