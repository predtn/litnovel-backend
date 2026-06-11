using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Comment;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateCommentReplyUseCase : ICreateCommentReplyUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateCommentRequestDto> _validator;

        public CreateCommentReplyUseCase(
            ICommentChapterRepository commentChapterRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IValidator<CreateCommentRequestDto> validator)
        {
            _commentChapterRepository = commentChapterRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<CommentResponseDto> ExecuteAsync(int commentId, CreateCommentRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var parent = await _commentChapterRepository.GetByIdAsync(commentId, ct)
                ?? throw new NotFoundException("Comment not found");

            var reply = new CommentChapter
            {
                ChapterId = parent.ChapterId,
                UserId = _currentUserService.UserId,
                ParentCommentId = parent.Id,
                Content = request.Content.Trim()
            };

            await _commentChapterRepository.AddAsync(reply, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CommentResponseDto
            {
                Id = reply.Id,
                User = new CommentUserResponseDto { Id = reply.UserId, Username = string.Empty },
                Content = reply.Content,
                LikeCount = reply.LikeCount,
                DislikeCount = reply.DislikeCount,
                ParentCommentId = reply.ParentCommentId,
                CreatedAt = reply.CreatedAt
            };
        }
    }
}
