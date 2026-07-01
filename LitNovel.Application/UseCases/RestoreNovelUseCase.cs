using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class RestoreNovelUseCase : IRestoreNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public RestoreNovelUseCase(
            INovelRepository novelRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateNovelResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!CanManage(novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to restore this novel");
            }

            if (novel.Status != NovelStatus.PendingDeletion)
            {
                throw new BadRequestException("Novel is not pending deletion");
            }

            novel.Status = novel.PreviousPublicStatus ?? NovelStatus.Ongoing;
            novel.PreviousPublicStatus = null;
            novel.DeletionRequestedAt = null;
            novel.ScheduledHardDeleteAt = null;
            novel.DeletionRequestedById = null;

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
