namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IAddCommentLikeUseCase
    {
        Task ExecuteAsync(int commentId, CancellationToken ct);
    }
}
