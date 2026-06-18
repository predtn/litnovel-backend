namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteAdminCategoryUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
