using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Tag;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        private readonly IGetTagsUseCase _getTagsUseCase;

        public TagsController(IGetTagsUseCase getTagsUseCase)
        {
            _getTagsUseCase = getTagsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _getTagsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<List<TagResponseDto>> { Success = true, Data = result });
        }
    }
}
