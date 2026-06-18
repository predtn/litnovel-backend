namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteAdminAnnouncementUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
