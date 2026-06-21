namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IMarkAllNotificationsReadUseCase
    {
        Task ExecuteAsync(CancellationToken ct);
    }
}
