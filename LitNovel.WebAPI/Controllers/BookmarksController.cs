using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/bookmarks")]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly IRemoveFavoriteUseCase _removeFavoriteUseCase;

        public BookmarksController(IRemoveFavoriteUseCase removeFavoriteUseCase)
        {
            _removeFavoriteUseCase = removeFavoriteUseCase;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _removeFavoriteUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }
    }
}
