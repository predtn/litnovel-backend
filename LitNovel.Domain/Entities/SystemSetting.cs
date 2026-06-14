using LitNovel.Domain.Common;

namespace LitNovel.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
