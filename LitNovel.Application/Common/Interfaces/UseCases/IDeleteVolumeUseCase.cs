namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IDeleteVolumeUseCase
    {
        Task ExecuteAsync(int id, CancellationToken ct);
    }
}
