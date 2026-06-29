using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class DeleteCommentUseCase : IDeleteCommentUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCommentUseCase(ICommentChapterRepository commentChapterRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _commentChapterRepository = commentChapterRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var comment = await _commentChapterRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Comment not found");

            var isAdmin = string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
            if (comment.UserId != _currentUserService.UserId && !isAdmin)
            {
                throw new ForbiddenException("You do not have permission to delete this comment");
            }

            _commentChapterRepository.Delete(comment);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
