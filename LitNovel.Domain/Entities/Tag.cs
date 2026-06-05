namespace LitNovel.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;

        public ICollection<NovelTag> NovelTags { get; set; } = new List<NovelTag>();
    }
}
