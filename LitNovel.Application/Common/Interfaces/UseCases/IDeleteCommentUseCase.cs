namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteCommentUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
