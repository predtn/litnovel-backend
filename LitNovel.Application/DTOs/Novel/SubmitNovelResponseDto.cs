namespace LitNovel.Application.DTOs.Novel
{
    public class SubmitNovelResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
