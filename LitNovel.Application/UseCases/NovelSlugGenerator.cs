using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LitNovel.Application.UseCases
{
    internal static partial class NovelSlugGenerator
    {
        public static string Generate(string title)
        {
            var normalized = title.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);

            foreach (var character in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(character);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            var slug = InvalidSlugCharacters().Replace(builder.ToString().Normalize(NormalizationForm.FormC), "-");
            slug = RepeatedHyphens().Replace(slug, "-").Trim('-');
            return string.IsNullOrWhiteSpace(slug) ? "novel" : slug;
        }

        [GeneratedRegex("[^a-z0-9]+")]
        private static partial Regex InvalidSlugCharacters();

        [GeneratedRegex("-+")]
        private static partial Regex RepeatedHyphens();
    }
}
