using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Tag;
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
    }
}
