using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Volume;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/volumes")]
    [Authorize]
    public class VolumesController : ControllerBase
    {
        private readonly IUpdateVolumeUseCase _updateVolumeUseCase;
        private readonly IDeleteVolumeUseCase _deleteVolumeUseCase;

        public VolumesController(
            IUpdateVolumeUseCase updateVolumeUseCase,
            IDeleteVolumeUseCase deleteVolumeUseCase)
        {
            _updateVolumeUseCase = updateVolumeUseCase;
            _deleteVolumeUseCase = deleteVolumeUseCase;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateVolumeRequestDto request, CancellationToken ct)
        {
            var result = await _updateVolumeUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<VolumeResponseDto> { Success = true, Message = "Volume updated successfully", Data = result });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteVolumeUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }
    }
}
