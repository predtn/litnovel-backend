namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteNovelUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
