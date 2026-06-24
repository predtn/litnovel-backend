using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.UseCases
{
    public class GetChapterCommentsUseCase : IGetChapterCommentsUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetChapterCommentsUseCase(
            IChapterRepository chapterRepository,
            ICommentChapterRepository commentChapterRepository,
            ICurrentUserService currentUserService)
        {
            _chapterRepository = chapterRepository;
            _commentChapterRepository = commentChapterRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<CommentResponseDto>> ExecuteAsync(int chapterId, int page, int size, CancellationToken ct)
        {
            _ = await _chapterRepository.GetByIdWithDetailsAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found");

            var currentUserId = _currentUserService.IsAuthenticated ? _currentUserService.UserId : (int?)null;
            return await _commentChapterRepository.GetByChapterAsync(chapterId, page, size, currentUserId, ct);
        }
    }
}
