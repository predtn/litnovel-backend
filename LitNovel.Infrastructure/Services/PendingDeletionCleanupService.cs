using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LitNovel.Infrastructure.Services
{
    public class PendingDeletionCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IPendingDeletionSettingsProvider _pendingDeletionSettingsProvider;
        private readonly ILogger<PendingDeletionCleanupService> _logger;

        public PendingDeletionCleanupService(
            IServiceScopeFactory scopeFactory,
            IPendingDeletionSettingsProvider pendingDeletionSettingsProvider,
            ILogger<PendingDeletionCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _pendingDeletionSettingsProvider = pendingDeletionSettingsProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var settings = _pendingDeletionSettingsProvider.GetSettings();
            await Task.Delay(TimeSpan.FromSeconds(settings.InitialDelaySeconds), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await PurgeExpiredContentAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(settings.CleanupIntervalSeconds), stoppingToken);
            }
        }

        private async Task PurgeExpiredContentAsync(CancellationToken ct)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var purgeUseCase = scope.ServiceProvider.GetRequiredService<IPurgePendingDeletionUseCase>();
                await purgeUseCase.ExecuteAsync(DateTime.UtcNow, ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to purge pending deletion content.");
            }
        }
    }
}
