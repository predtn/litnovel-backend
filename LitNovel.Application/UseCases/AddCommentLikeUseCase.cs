using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class AddCommentLikeUseCase : IAddCommentLikeUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddCommentLikeUseCase(ICommentChapterRepository commentChapterRepository, ICommentLikeRepository commentLikeRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
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

            if (await _commentLikeRepository.GetAsync(_currentUserService.UserId, commentId, ct) is not null)
            {
                throw new ConflictException("Comment already liked");
            }

            await _commentLikeRepository.AddAsync(new CommentLike { UserId = _currentUserService.UserId, CommentChapterId = commentId }, ct);
            comment.LikeCount++;
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
