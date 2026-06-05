namespace LitNovel.Domain.Entities
{
    public class Volume
    {
        public int Id { get; set; }
        public int NovelId { get; set; }
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;

        public Novel Novel { get; set; } = default!;
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
