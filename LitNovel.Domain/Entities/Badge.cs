namespace LitNovel.Domain.Entities
{
    public class Badge
    {
        public int Id { get; set; }
        public string Key { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Icon { get; set; }
        public string? Color { get; set; }

        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    }
}
