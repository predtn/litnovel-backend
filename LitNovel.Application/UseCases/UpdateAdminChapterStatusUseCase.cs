using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminChapterStatusUseCase : IUpdateAdminChapterStatusUseCase
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAdminChapterStatusRequestDto> _validator;

        public UpdateAdminChapterStatusUseCase(
            IChapterRepository chapterRepository,
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateAdminChapterStatusRequestDto> validator)
        {
            _chapterRepository = chapterRepository;
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminChapterStatusResponseDto> ExecuteAsync(int id, UpdateAdminChapterStatusRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (id <= 0)
            {
                throw new BadRequestException("Invalid chapter id");
            }

            var chapter = await _chapterRepository.GetByIdForUpdateAsync(id, ct)
                ?? throw new NotFoundException("Chapter not found");

            chapter.Status = Enum.Parse<ChapterStatus>(request.Status, true);
            await _auditLogRepository.AddAsync(new AuditLog
            {
                ActorId = _currentUserService.UserId,
                Action = "UpdateChapterStatus",
                EntityType = "Chapter",
                EntityId = chapter.Id,
                IpAddress = _currentUserService.IpAddress
            }, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AdminChapterStatusResponseDto
            {
                ChapterId = chapter.Id,
                Status = chapter.Status.ToString(),
                UpdatedAt = chapter.UpdatedAt
            };
        }
    }
}
