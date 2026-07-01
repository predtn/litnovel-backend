namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IPurgePendingDeletionUseCase
    {
        Task ExecuteAsync(DateTime utcNow, CancellationToken ct);
    }
}
