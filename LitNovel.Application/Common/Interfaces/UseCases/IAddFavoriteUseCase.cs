namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IAddFavoriteUseCase
    {
        Task ExecuteAsync(int novelId, CancellationToken ct);
    }
}
