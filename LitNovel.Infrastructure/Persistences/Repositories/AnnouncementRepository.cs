using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly LitNovelContext _context;

        public AnnouncementRepository(LitNovelContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<AdminAnnouncementResponseDto>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Announcements
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AdminAnnouncementResponseDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync(ct);
        }

        public Task<Announcement?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.Announcements.FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public Task AddAsync(Announcement announcement, CancellationToken ct)
        {
            return _context.Announcements.AddAsync(announcement, ct).AsTask();
        }

        public void Delete(Announcement announcement)
        {
            _context.Announcements.Remove(announcement);
        }
    }
}
