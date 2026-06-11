namespace LitNovel.Application.DTOs.Volume
{
    public class UpdateVolumeRequestDto
    {
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;
    }
}
