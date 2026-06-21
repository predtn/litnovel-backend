using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public static class AdminAnnouncementMapper
    {
        public static AdminAnnouncementResponseDto Map(Announcement announcement)
        {
            return new AdminAnnouncementResponseDto
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Content = announcement.Content,
                StartDate = announcement.StartDate,
                EndDate = announcement.EndDate,
                IsActive = announcement.IsActive,
                CreatedAt = announcement.CreatedAt,
                UpdatedAt = announcement.UpdatedAt
            };
        }

        public static AdminAnnouncementSummaryResponseDto MapSummary(Announcement announcement)
        {
            return new AdminAnnouncementSummaryResponseDto
            {
                Id = announcement.Id,
                Title = announcement.Title,
                IsActive = announcement.IsActive,
                StartDate = announcement.StartDate,
                EndDate = announcement.EndDate,
                CreatedAt = announcement.CreatedAt
            };
        }
    }
}
