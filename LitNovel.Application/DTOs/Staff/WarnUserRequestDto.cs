namespace LitNovel.Application.DTOs.Staff
{
    public class WarnUserRequestDto
    {
        public int UserId { get; set; }
        public string Message { get; set; } = default!;
    }
}
