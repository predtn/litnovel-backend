namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteChapterUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
