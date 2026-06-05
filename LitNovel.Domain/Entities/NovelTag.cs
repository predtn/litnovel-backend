namespace LitNovel.Domain.Entities
{
    public class NovelTag
    {
        public int NovelId { get; set; }
        public int TagId { get; set; }

        public Novel Novel { get; set; } = default!;
        public Tag Tag { get; set; } = default!;
    }
}
