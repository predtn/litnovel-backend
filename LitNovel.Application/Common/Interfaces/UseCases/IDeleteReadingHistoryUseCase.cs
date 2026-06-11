namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteReadingHistoryUseCase
    {
        Task ExecuteAsync(int novelId, CancellationToken ct);
    }
}
