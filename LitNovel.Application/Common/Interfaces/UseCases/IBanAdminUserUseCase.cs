using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IBanAdminUserUseCase
    {
        Task<BanAdminUserResponseDto> ExecuteAsync(int id, BanAdminUserRequestDto request, CancellationToken ct);
    }
}
