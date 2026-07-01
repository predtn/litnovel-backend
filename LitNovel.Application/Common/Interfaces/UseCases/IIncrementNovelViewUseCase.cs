namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IIncrementNovelViewUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
