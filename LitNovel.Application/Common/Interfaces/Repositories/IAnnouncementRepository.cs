using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.Common.Interfaces.Repositories
{
    public interface IAnnouncementRepository
    {
        Task<IReadOnlyList<AdminAnnouncementResponseDto>> GetAllAsync(CancellationToken ct);
        Task<Announcement?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Announcement announcement, CancellationToken ct);
        void Delete(Announcement announcement);
    }
}
