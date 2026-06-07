namespace LitNovel.Application.DTOs.Volume
{
    public class CreateVolumeRequestDto
    {
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;
    }
}
