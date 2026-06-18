using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminAnnouncementUseCase : IUpdateAdminAnnouncementUseCase
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAdminAnnouncementRequestDto> _validator;

        public UpdateAdminAnnouncementUseCase(
            IAnnouncementRepository announcementRepository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateAdminAnnouncementRequestDto> validator)
        {
            _announcementRepository = announcementRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminAnnouncementResponseDto> ExecuteAsync(int id, UpdateAdminAnnouncementRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var announcement = await _announcementRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Announcement not found");

            var newStartDate = request.StartDate ?? announcement.StartDate;
            var newEndDate = request.EndDate ?? announcement.EndDate;
            if (newEndDate.HasValue && newEndDate.Value <= newStartDate)
            {
                throw new BadRequestException("End date must be after start date");
            }

            if (request.Title is not null)
            {
                announcement.Title = request.Title.Trim();
            }

            if (request.Content is not null)
            {
                announcement.Content = request.Content.Trim();
            }

            if (request.StartDate.HasValue)
            {
                announcement.StartDate = request.StartDate.Value;
            }

            if (request.EndDate.HasValue)
            {
                announcement.EndDate = request.EndDate;
            }

            if (request.IsActive.HasValue)
            {
                announcement.IsActive = request.IsActive.Value;
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return AdminAnnouncementMapper.Map(announcement);
        }
    }
}
