namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteNovelReviewUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
