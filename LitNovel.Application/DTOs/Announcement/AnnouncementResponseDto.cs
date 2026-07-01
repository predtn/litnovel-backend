namespace LitNovel.Application.DTOs.Announcement
{
    public class AnnouncementResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
