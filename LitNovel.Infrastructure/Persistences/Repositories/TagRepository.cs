using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Tag;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly LitNovelContext _context;

        public TagRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task<List<TagResponseDto>> GetAllAsync(CancellationToken ct)
        {
            return _context.Tags
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Select(t => new TagResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Slug = t.Slug
                })
                .ToListAsync(ct);
        }

        public Task<List<int>> GetExistingIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct)
        {
            return _context.Tags
                .AsNoTracking()
                .Where(t => ids.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(ct);
        }

        public Task<Tag?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.Tags.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public Task<bool> NameExistsAsync(string name, int? excludingId, CancellationToken ct)
        {
            var normalized = name.Trim();
            return _context.Tags
                .AsNoTracking()
                .AnyAsync(t => t.Name == normalized && (!excludingId.HasValue || t.Id != excludingId.Value), ct);
        }

        public Task<bool> SlugExistsAsync(string slug, int? excludingId, CancellationToken ct)
        {
            var normalized = slug.Trim();
            return _context.Tags
                .AsNoTracking()
                .AnyAsync(t => t.Slug == normalized && (!excludingId.HasValue || t.Id != excludingId.Value), ct);
        }

        public Task AddAsync(Tag tag, CancellationToken ct)
        {
            return _context.Tags.AddAsync(tag, ct).AsTask();
        }

        public void Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
        }
    }
}
