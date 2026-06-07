using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class GetNovelUseCase : IGetNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetNovelUseCase(INovelRepository novelRepository, ICurrentUserService currentUserService)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
        }

        public async Task<NovelDetailResponseDto> ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdWithDetailsAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!CanView(novel))
            {
                throw new ForbiddenException("Novel is not publicly available");
            }

            return new NovelDetailResponseDto
            {
                Id = novel.Id,
                Title = novel.Title,
                Slug = novel.Slug,
                CoverImage = novel.CoverImage,
                Description = novel.Description,
                Author = new NovelAuthorResponseDto
                {
                    Id = novel.Author.Id,
                    Username = novel.Author.Username,
                    Avatar = novel.Author.Avatar
                },
                Category = novel.Category == null ? null : new NovelCategoryResponseDto
                {
                    Id = novel.Category.Id,
                    Name = novel.Category.Name
                },
                Tags = novel.NovelTags
                    .Select(nt => new NovelTagResponseDto { Id = nt.Tag.Id, Name = nt.Tag.Name })
                    .ToList(),
                Status = novel.Status.ToString(),
                ViewCount = novel.ViewCount,
                LikeCount = novel.LikeCount,
                TotalChapters = novel.TotalChapters,
                TotalVolumes = novel.TotalVolumes,
                RatingAverage = novel.NovelRatings.Any() ? novel.NovelRatings.Average(r => r.Rating) : 0,
                RatingCount = novel.NovelRatings.Count,
                CreatedAt = novel.CreatedAt,
                UpdatedAt = novel.UpdatedAt
            };
        }

        private bool CanView(Novel novel)
        {
            if (IsPublicStatus(novel.Status))
            {
                return true;
            }

            return _currentUserService.IsAuthenticated
                && (novel.AuthorId == _currentUserService.UserId
                    || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                    || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsPublicStatus(NovelStatus status)
        {
            return status is NovelStatus.Ongoing or NovelStatus.Ended or NovelStatus.Hiatus or NovelStatus.Dropped;
        }
    }
}
