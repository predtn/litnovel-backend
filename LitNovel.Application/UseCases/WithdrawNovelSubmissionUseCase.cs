using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class WithdrawNovelSubmissionUseCase : IWithdrawNovelSubmissionUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public WithdrawNovelSubmissionUseCase(
            INovelRepository novelRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<SubmitNovelResponseDto> ExecuteAsync(int id, CancellationToken ct)
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

            if (!VolumePermissionHelper.CanManage(_currentUserService, novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (novel.Status != NovelStatus.Pending)
            {
                throw new BadRequestException("Novel must be in Pending status to withdraw");
            }

            novel.Status = NovelStatus.Draft;
            await _unitOfWork.SaveChangesAsync(ct);

            return new SubmitNovelResponseDto
            {
                Id = novel.Id,
                Status = novel.Status.ToString(),
                UpdatedAt = novel.UpdatedAt
            };
        }
    }
}
