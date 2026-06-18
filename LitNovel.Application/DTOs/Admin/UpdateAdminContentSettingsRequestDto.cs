namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminContentSettingsRequestDto
    {
        public int? MaxNovelDescriptionLength { get; set; }
        public int? MaxChapterLength { get; set; }
        public int? MaxTagsPerNovel { get; set; }
    }
}
