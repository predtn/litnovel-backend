using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class ToggleAdminAnnouncementUseCase : IToggleAdminAnnouncementUseCase
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ToggleAdminAnnouncementUseCase(IAnnouncementRepository announcementRepository, IUnitOfWork unitOfWork)
        {
            _announcementRepository = announcementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminAnnouncementResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Announcement not found");

            announcement.IsActive = !announcement.IsActive;
            await _unitOfWork.SaveChangesAsync(ct);

            return AdminAnnouncementMapper.Map(announcement);
        }
    }
}
