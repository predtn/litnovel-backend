namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRemoveNovelLikeUseCase
    {
        Task ExecuteAsync(int novelId, CancellationToken ct);
    }
}
