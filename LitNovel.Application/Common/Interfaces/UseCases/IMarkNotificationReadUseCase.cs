namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IMarkNotificationReadUseCase
    {
        Task ExecuteAsync(int notificationId, CancellationToken ct);
    }
}
