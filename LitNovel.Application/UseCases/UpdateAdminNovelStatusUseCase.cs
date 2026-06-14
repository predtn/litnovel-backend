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
    public class UpdateAdminNovelStatusUseCase : IUpdateAdminNovelStatusUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAdminNovelStatusRequestDto> _validator;

        public UpdateAdminNovelStatusUseCase(
            INovelRepository novelRepository,
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateAdminNovelStatusRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminNovelStatusResponseDto> ExecuteAsync(int id, UpdateAdminNovelStatusRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(id, ct)
                ?? throw new NotFoundException("Novel not found");

            novel.Status = Enum.Parse<NovelStatus>(request.Status, true);
            await _auditLogRepository.AddAsync(new AuditLog
            {
                ActorId = _currentUserService.UserId,
                Action = "UpdateNovelStatus",
                EntityType = "Novel",
                EntityId = novel.Id,
                IpAddress = _currentUserService.IpAddress
            }, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AdminNovelStatusResponseDto
            {
                NovelId = novel.Id,
                Status = novel.Status.ToString(),
                UpdatedAt = novel.UpdatedAt
            };
        }
    }
}
