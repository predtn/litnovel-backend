using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminNovelAuthorUseCase : IUpdateAdminNovelAuthorUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAdminNovelAuthorRequestDto> _validator;

        public UpdateAdminNovelAuthorUseCase(
            INovelRepository novelRepository,
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<UpdateAdminNovelAuthorRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _userRepository = userRepository;
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminNovelAuthorResponseDto> ExecuteAsync(int id, UpdateAdminNovelAuthorRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(id, ct)
                ?? throw new NotFoundException("Novel not found");

            var author = await _userRepository.GetByIdAsync(request.AuthorId, ct)
                ?? throw new NotFoundException("Author not found");

            if (await _novelRepository.TitleExistsForAuthorAsync(author.Id, novel.Title, novel.Id, ct))
            {
                throw new ConflictException("Novel title already exists for the target author");
            }

            novel.AuthorId = author.Id;
            await _auditLogRepository.AddAsync(new AuditLog
            {
                ActorId = _currentUserService.UserId,
                Action = "UpdateNovelAuthor",
                EntityType = "Novel",
                EntityId = novel.Id,
                IpAddress = _currentUserService.IpAddress
            }, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AdminNovelAuthorResponseDto
            {
                NovelId = novel.Id,
                AuthorId = novel.AuthorId,
                UpdatedAt = novel.UpdatedAt
            };
        }
    }
}
