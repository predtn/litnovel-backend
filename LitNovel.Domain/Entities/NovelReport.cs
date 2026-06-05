using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    public class NovelReport : BaseReport
    {
        public int TargetNovelId { get; set; }
        public int? TargetChapterId { get; set; }

        public Novel TargetNovel { get; set; } = default!;
        public Chapter? TargetChapter { get; set; }
    }
}
