namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteAdminTagUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
