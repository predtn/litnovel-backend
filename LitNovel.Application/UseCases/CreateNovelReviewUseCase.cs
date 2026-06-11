using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Review;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateNovelReviewUseCase : ICreateNovelReviewUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly INovelRatingRepository _novelRatingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateNovelReviewRequestDto> _validator;

        public CreateNovelReviewUseCase(
            INovelRepository novelRepository,
            INovelRatingRepository novelRatingRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<CreateNovelReviewRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _novelRatingRepository = novelRatingRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<NovelReviewResponseDto> ExecuteAsync(int novelId, CreateNovelReviewRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var novel = await _novelRepository.GetByIdWithDetailsAsync(novelId, ct)
                ?? throw new NotFoundException("Novel not found");

            if (novel.AuthorId == _currentUserService.UserId)
            {
                throw new BadRequestException("Cannot review your own novel");
            }

            if (await _novelRatingRepository.GetByUserAndNovelAsync(_currentUserService.UserId, novelId, ct) is not null)
            {
                throw new ConflictException("You have already reviewed this novel");
            }

            var rating = new NovelRating
            {
                NovelId = novelId,
                UserId = _currentUserService.UserId,
                Rating = (byte)request.Rating,
                Review = request.Review
            };

            await _novelRatingRepository.AddAsync(rating, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new NovelReviewResponseDto
            {
                Id = rating.Id,
                User = new NovelReviewUserResponseDto
                {
                    Id = _currentUserService.UserId,
                    Username = string.Empty
                },
                Rating = rating.Rating,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }
    }
}
