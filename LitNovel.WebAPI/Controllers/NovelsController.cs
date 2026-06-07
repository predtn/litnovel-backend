using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/novels")]
    public class NovelsController : ControllerBase
    {
        private readonly IGetNovelsUseCase _getNovelsUseCase;

        public NovelsController(IGetNovelsUseCase getNovelsUseCase)
        {
            _getNovelsUseCase = getNovelsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] NovelListQueryDto query, CancellationToken ct)
        {
            var result = await _getNovelsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<NovelListItemResponseDto>> { Success = true, Data = result });
        }
    }
}
