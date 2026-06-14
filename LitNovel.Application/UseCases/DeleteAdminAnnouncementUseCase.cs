using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteAdminAnnouncementUseCase : IDeleteAdminAnnouncementUseCase
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAdminAnnouncementUseCase(IAnnouncementRepository announcementRepository, IUnitOfWork unitOfWork)
        {
            _announcementRepository = announcementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Announcement not found");

            _announcementRepository.Delete(announcement);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
