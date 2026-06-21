namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteNotificationUseCase
    {
        Task ExecuteAsync(int notificationId, CancellationToken ct);
    }
}
