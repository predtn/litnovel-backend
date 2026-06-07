using LitNovel.Application.DTOs.User;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    internal static class UserProfileMapper
    {
        public static MyProfileResponseDto ToMyProfile(User user)
        {
            return new MyProfileResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar,
                Bio = user.Bio,
                Role = user.Role.ToString(),
                Status = user.Status.ToString(),
                Reputation = user.Reputation?.Score ?? 0,
                Badges = MapBadges(user),
                Stats = MapStats(user),
                CreatedAt = user.CreatedAt
            };
        }

        public static PublicProfileResponseDto ToPublicProfile(User user)
        {
            return new PublicProfileResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Avatar = user.Avatar,
                Bio = user.Bio,
                Reputation = user.Reputation?.Score ?? 0,
                Badges = MapBadges(user),
                Stats = MapStats(user),
                JoinedAt = user.CreatedAt
            };
        }

        private static List<BadgeResponseDto> MapBadges(User user)
        {
            return user.UserBadges
                .Select(ub => new BadgeResponseDto
                {
                    Key = ub.Badge.Key,
                    Name = ub.Badge.Name,
                    Icon = ub.Badge.Icon,
                    Color = ub.Badge.Color,
                    EarnedAt = ub.EarnedAt
                })
                .ToList();
        }

        private static UserStatsResponseDto MapStats(User user)
        {
            return new UserStatsResponseDto
            {
                NovelsCreated = user.Novels.Count,
                NovelsPublished = user.Novels.Count(n => n.Status != NovelStatus.Canceled),
                ChaptersPublished = user.Novels.SelectMany(n => n.Volumes).SelectMany(v => v.Chapters).Count(c => c.Status == ChapterStatus.Published),
                FavoritesCount = user.Favorites.Count,
                CommentsCount = user.CommentChapters.Count
            };
        }
    }
}
