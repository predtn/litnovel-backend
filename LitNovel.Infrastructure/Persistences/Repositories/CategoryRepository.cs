using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Category;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LitNovelContext _context;

        public CategoryRepository(LitNovelContext context)
        {
            _context = context;
        }

        public Task<List<CategoryResponseDto>> GetAllAsync(CancellationToken ct)
        {
            return _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug
                })
                .ToListAsync(ct);
        }

        public Task<Category?> GetByIdAsync(int id, CancellationToken ct)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return _context.Categories.AnyAsync(c => c.Id == id, ct);
        }

        public Task<bool> NameExistsAsync(string name, int? excludingId, CancellationToken ct)
        {
            var normalized = name.Trim();
            return _context.Categories
                .AsNoTracking()
                .AnyAsync(c => c.Name == normalized && (!excludingId.HasValue || c.Id != excludingId.Value), ct);
        }

        public Task<bool> SlugExistsAsync(string slug, int? excludingId, CancellationToken ct)
        {
            var normalized = slug.Trim();
            return _context.Categories
                .AsNoTracking()
                .AnyAsync(c => c.Slug == normalized && (!excludingId.HasValue || c.Id != excludingId.Value), ct);
        }

        public Task AddAsync(Category category, CancellationToken ct)
        {
            return _context.Categories.AddAsync(category, ct).AsTask();
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }
    }
}
