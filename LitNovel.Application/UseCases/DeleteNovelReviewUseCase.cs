using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteNovelReviewUseCase : IDeleteNovelReviewUseCase
    {
        private readonly INovelRatingRepository _novelRatingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteNovelReviewUseCase(INovelRatingRepository novelRatingRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _novelRatingRepository = novelRatingRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var rating = await _novelRatingRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Review not found");

            if (rating.UserId != _currentUserService.UserId)
            {
                throw new ForbiddenException("You do not have permission to delete this review");
            }

            _novelRatingRepository.Delete(rating);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
