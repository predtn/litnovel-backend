namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteAdminUserUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
