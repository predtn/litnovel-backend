using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminUserUseCase
    {
        Task<AdminUserUpdateResponseDto> ExecuteAsync(int id, UpdateAdminUserRequestDto request, CancellationToken ct);
    }
}
