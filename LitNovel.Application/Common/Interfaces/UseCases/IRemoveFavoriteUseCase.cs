namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRemoveFavoriteUseCase
    {
        Task ExecuteAsync(int novelId, CancellationToken ct);
    }
}
