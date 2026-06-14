using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminSettingsUseCase : IGetAdminSettingsUseCase
    {
        private readonly ISystemSettingRepository _systemSettingRepository;

        public GetAdminSettingsUseCase(ISystemSettingRepository systemSettingRepository)
        {
            _systemSettingRepository = systemSettingRepository;
        }

        public async Task<AdminSettingsResponseDto> ExecuteAsync(CancellationToken ct)
        {
            var settings = await _systemSettingRepository.GetAllAsync(ct);
            return AdminSettingsMapper.ToResponse(settings);
        }
    }
}
