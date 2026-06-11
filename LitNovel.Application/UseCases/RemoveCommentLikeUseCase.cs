using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class RemoveCommentLikeUseCase : IRemoveCommentLikeUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveCommentLikeUseCase(ICommentChapterRepository commentChapterRepository, ICommentLikeRepository commentLikeRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _commentChapterRepository = commentChapterRepository;
            _commentLikeRepository = commentLikeRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int commentId, CancellationToken ct)
        {
            var comment = await _commentChapterRepository.GetByIdAsync(commentId, ct)
                ?? throw new NotFoundException("Comment not found");

            var like = await _commentLikeRepository.GetAsync(_currentUserService.UserId, commentId, ct)
                ?? throw new NotFoundException("Comment like not found");

            _commentLikeRepository.Delete(like);
            comment.LikeCount = Math.Max(0, comment.LikeCount - 1);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
