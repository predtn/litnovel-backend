using System.Text.Json;
using LitNovel.Application.DTOs.Admin;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    internal static class AdminSettingsMapper
    {
        private const string SiteNameKey = "general.siteName";
        private const string TaglineKey = "general.tagline";
        private const string MaintenanceModeKey = "general.maintenanceMode";
        private const string MaxNovelDescriptionLengthKey = "content.maxNovelDescriptionLength";
        private const string MaxChapterLengthKey = "content.maxChapterLength";
        private const string MaxTagsPerNovelKey = "content.maxTagsPerNovel";
        private const string ReviewSLAHoursKey = "moderation.reviewSLAHours";
        private const string AutoFlagKeywordsKey = "moderation.autoFlagKeywords";

        public static AdminSettingsResponseDto ToResponse(IEnumerable<SystemSetting> settings)
        {
            var values = settings.ToDictionary(setting => setting.Key, setting => setting.Value);

            return new AdminSettingsResponseDto
            {
                General = new AdminGeneralSettingsResponseDto
                {
                    SiteName = GetString(values, SiteNameKey, "LitNovel"),
                    Tagline = GetString(values, TaglineKey, "Read, Write, Discover"),
                    MaintenanceMode = GetBool(values, MaintenanceModeKey, false)
                },
                Content = new AdminContentSettingsResponseDto
                {
                    MaxNovelDescriptionLength = GetInt(values, MaxNovelDescriptionLengthKey, 5000),
                    MaxChapterLength = GetInt(values, MaxChapterLengthKey, 50000),
                    MaxTagsPerNovel = GetInt(values, MaxTagsPerNovelKey, 10)
                },
                Moderation = new AdminModerationSettingsResponseDto
                {
                    ReviewSLAHours = GetInt(values, ReviewSLAHoursKey, 48),
                    AutoFlagKeywords = GetKeywords(values, AutoFlagKeywordsKey)
                }
            };
        }

        public static IReadOnlyDictionary<string, string> ToSettingValues(UpdateAdminSettingsRequestDto request)
        {
            return new Dictionary<string, string>
            {
                [SiteNameKey] = request.General!.SiteName!.Trim(),
                [TaglineKey] = request.General.Tagline?.Trim() ?? string.Empty,
                [MaintenanceModeKey] = request.General.MaintenanceMode!.Value.ToString(),
                [MaxNovelDescriptionLengthKey] = request.Content!.MaxNovelDescriptionLength!.Value.ToString(),
                [MaxChapterLengthKey] = request.Content.MaxChapterLength!.Value.ToString(),
                [MaxTagsPerNovelKey] = request.Content.MaxTagsPerNovel!.Value.ToString(),
                [ReviewSLAHoursKey] = request.Moderation!.ReviewSLAHours!.Value.ToString(),
                [AutoFlagKeywordsKey] = JsonSerializer.Serialize(request.Moderation.AutoFlagKeywords ?? Array.Empty<string>())
            };
        }

        private static string GetString(IReadOnlyDictionary<string, string> values, string key, string defaultValue)
        {
            return values.TryGetValue(key, out var value) ? value : defaultValue;
        }

        private static int GetInt(IReadOnlyDictionary<string, string> values, string key, int defaultValue)
        {
            return values.TryGetValue(key, out var value) && int.TryParse(value, out var parsed)
                ? parsed
                : defaultValue;
        }

        private static bool GetBool(IReadOnlyDictionary<string, string> values, string key, bool defaultValue)
        {
            return values.TryGetValue(key, out var value) && bool.TryParse(value, out var parsed)
                ? parsed
                : defaultValue;
        }

        private static IReadOnlyList<string> GetKeywords(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
            {
                return Array.Empty<string>();
            }

            try
            {
                return JsonSerializer.Deserialize<IReadOnlyList<string>>(value) ?? Array.Empty<string>();
            }
            catch (JsonException)
            {
                return Array.Empty<string>();
            }
        }
    }
}
