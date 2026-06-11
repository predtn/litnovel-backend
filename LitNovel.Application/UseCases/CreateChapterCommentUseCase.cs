using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Comment;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateChapterCommentUseCase : ICreateChapterCommentUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateCommentRequestDto> _validator;

        public CreateChapterCommentUseCase(
            IChapterRepository chapterRepository,
            ICommentChapterRepository commentChapterRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<CreateCommentRequestDto> validator)
        {
            _chapterRepository = chapterRepository;
            _commentChapterRepository = commentChapterRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<CommentResponseDto> ExecuteAsync(int chapterId, CreateCommentRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            _ = await _chapterRepository.GetByIdWithDetailsAsync(chapterId, ct)
                ?? throw new NotFoundException("Chapter not found");

            var comment = new CommentChapter
            {
                ChapterId = chapterId,
                UserId = _currentUserService.UserId,
                Content = request.Content.Trim()
            };

            await _commentChapterRepository.AddAsync(comment, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return MapCreated(comment);
        }

        private static CommentResponseDto MapCreated(CommentChapter comment)
        {
            return new CommentResponseDto
            {
                Id = comment.Id,
                User = new CommentUserResponseDto { Id = comment.UserId, Username = string.Empty },
                Content = comment.Content,
                LikeCount = comment.LikeCount,
                DislikeCount = comment.DislikeCount,
                ParentCommentId = comment.ParentCommentId,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
