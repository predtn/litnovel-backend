using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Comment;

namespace LitNovel.Application.UseCases
{
    public class UpdateCommentUseCase : IUpdateCommentUseCase
    {
        private readonly ICommentChapterRepository _commentChapterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateCommentRequestDto> _validator;

        public UpdateCommentUseCase(ICommentChapterRepository commentChapterRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IValidator<CreateCommentRequestDto> validator)
        {
            _commentChapterRepository = commentChapterRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<CommentResponseDto> ExecuteAsync(int id, CreateCommentRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var comment = await _commentChapterRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Comment not found");

            if (comment.UserId != _currentUserService.UserId)
            {
                throw new ForbiddenException("You do not have permission to edit this comment");
            }

            comment.Content = request.Content.Trim();
            await _unitOfWork.SaveChangesAsync(ct);

            return new CommentResponseDto
            {
                Id = comment.Id,
                User = new CommentUserResponseDto
                {
                    Id = comment.UserId,
                    Username = comment.User.Username,
                    Avatar = comment.User.Avatar
                },
                Content = comment.Content,
                LikeCount = comment.LikeCount,
                DislikeCount = comment.DislikeCount,
                ParentCommentId = comment.ParentCommentId,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
