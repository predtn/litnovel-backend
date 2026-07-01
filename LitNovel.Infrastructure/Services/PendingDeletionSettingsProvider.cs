using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Models;
using Microsoft.Extensions.Configuration;

namespace LitNovel.Infrastructure.Services
{
    public class PendingDeletionSettingsProvider : IPendingDeletionSettingsProvider
    {
        private const int DefaultRetentionSeconds = 30 * 24 * 60 * 60;
        private const int DefaultInitialDelaySeconds = 60;
        private const int DefaultCleanupIntervalSeconds = 6 * 60 * 60;
        private readonly IConfiguration _configuration;

        public PendingDeletionSettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PendingDeletionSettings GetSettings()
        {
            var section = _configuration.GetSection("PendingDeletion");

            return new PendingDeletionSettings
            {
                RetentionSeconds = GetPositiveInt(section, "RetentionSeconds", DefaultRetentionSeconds),
                InitialDelaySeconds = GetPositiveInt(section, "InitialDelaySeconds", DefaultInitialDelaySeconds),
                CleanupIntervalSeconds = GetPositiveInt(section, "CleanupIntervalSeconds", DefaultCleanupIntervalSeconds)
            };
        }

        private static int GetPositiveInt(IConfiguration section, string key, int defaultValue)
        {
            var value = section.GetValue<int?>(key);
            return value.HasValue && value.Value > 0 ? value.Value : defaultValue;
        }
    }
}
