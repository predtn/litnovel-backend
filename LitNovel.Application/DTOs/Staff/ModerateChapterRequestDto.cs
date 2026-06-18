namespace LitNovel.Application.DTOs.Staff
{
    public class ModerateChapterRequestDto
    {
        /// <summary>Approve | Reject | Lock</summary>
        public string Action { get; set; } = default!;
        public string? Reason { get; set; }
    }
}
