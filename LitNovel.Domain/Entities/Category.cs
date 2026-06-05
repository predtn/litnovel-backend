namespace LitNovel.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;

        public ICollection<Novel> Novels { get; set; } = new List<Novel>();
    }
}
