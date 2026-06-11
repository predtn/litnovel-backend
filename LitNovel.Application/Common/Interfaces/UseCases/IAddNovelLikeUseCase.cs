namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IAddNovelLikeUseCase
    {
        Task ExecuteAsync(int novelId, CancellationToken ct);
    }
}
