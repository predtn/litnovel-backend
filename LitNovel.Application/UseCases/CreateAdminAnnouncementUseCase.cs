using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateAdminAnnouncementUseCase : ICreateAdminAnnouncementUseCase
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateAdminAnnouncementRequestDto> _validator;

        public CreateAdminAnnouncementUseCase(
            IAnnouncementRepository announcementRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateAdminAnnouncementRequestDto> validator)
        {
            _announcementRepository = announcementRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<AdminAnnouncementSummaryResponseDto> ExecuteAsync(CreateAdminAnnouncementRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var announcement = new Announcement
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                StartDate = request.StartDate!.Value,
                EndDate = request.EndDate,
                IsActive = request.IsActive!.Value
            };

            await _announcementRepository.AddAsync(announcement, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return AdminAnnouncementMapper.MapSummary(announcement);
        }
    }
}
