namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteAdminBadgeUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
