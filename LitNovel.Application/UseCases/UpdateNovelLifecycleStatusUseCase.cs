using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class UpdateNovelLifecycleStatusUseCase : IUpdateNovelLifecycleStatusUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateNovelLifecycleStatusRequestDto> _validator;

        public UpdateNovelLifecycleStatusUseCase(
            INovelRepository novelRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateNovelLifecycleStatusRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<UpdateNovelResponseDto> ExecuteAsync(int id, UpdateNovelLifecycleStatusRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            if (!Enum.TryParse<NovelStatus>(request.Status.Trim(), true, out var requestedStatus))
            {
                throw new BadRequestException("Lifecycle status must be one of: Ongoing, Ended, Hiatus, Dropped");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(id, ct)
                ?? throw new NotFoundException("Novel not found");

            if (!CanManage(novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (novel.Status == NovelStatus.Pending)
            {
                throw new BadRequestException("Cannot change lifecycle status while novel is pending review");
            }

            if (novel.Status == NovelStatus.Locked)
            {
                throw new BadRequestException("Locked novel cannot be edited");
            }

            if (novel.Status is NovelStatus.Draft or NovelStatus.Canceled)
            {
                throw new BadRequestException("Only published novels can change lifecycle status");
            }

            novel.Status = requestedStatus;
            novel.PreviousPublicStatus = null;

            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateNovelResponseDto
            {
                Id = novel.Id,
                Title = novel.Title,
                Slug = novel.Slug,
                Status = novel.Status.ToString(),
                UpdatedAt = novel.UpdatedAt
            };
        }

        private bool CanManage(int authorId)
        {
            return authorId == _currentUserService.UserId
                || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
        }

    }
}
