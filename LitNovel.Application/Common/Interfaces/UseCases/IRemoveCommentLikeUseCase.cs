namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRemoveCommentLikeUseCase
    {
        Task ExecuteAsync(int commentId, CancellationToken ct);
    }
}
