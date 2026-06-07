using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.DTOs.Category;
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
    }
}
