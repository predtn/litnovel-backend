namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteAdminBackupUseCase
    {
        Task ExecuteAsync(string id, CancellationToken ct);
    }
}
