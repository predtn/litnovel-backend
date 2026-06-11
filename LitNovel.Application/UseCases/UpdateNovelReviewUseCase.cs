using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Review;

namespace LitNovel.Application.UseCases
{
    public class UpdateNovelReviewUseCase : IUpdateNovelReviewUseCase
    {
        private readonly INovelRatingRepository _novelRatingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<UpdateNovelReviewRequestDto> _validator;

        public UpdateNovelReviewUseCase(INovelRatingRepository novelRatingRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IValidator<UpdateNovelReviewRequestDto> validator)
        {
            _novelRatingRepository = novelRatingRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<NovelReviewResponseDto> ExecuteAsync(int id, UpdateNovelReviewRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var rating = await _novelRatingRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Review not found");

            if (rating.UserId != _currentUserService.UserId)
            {
                throw new ForbiddenException("You do not have permission to edit this review");
            }

            rating.Rating = (byte)request.Rating;
            rating.Review = request.Review;
            rating.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(ct);

            return new NovelReviewResponseDto
            {
                Id = rating.Id,
                User = new NovelReviewUserResponseDto
                {
                    Id = rating.UserId,
                    Username = rating.User.Username,
                    Avatar = rating.User.Avatar
                },
                Rating = rating.Rating,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }
    }
}
