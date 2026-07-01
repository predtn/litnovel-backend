using LitNovel.Application.Common.Models;

namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface IPendingDeletionSettingsProvider
    {
        PendingDeletionSettings GetSettings();
    }
}
