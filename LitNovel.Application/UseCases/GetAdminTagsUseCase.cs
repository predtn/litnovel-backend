using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.UseCases
{
    public class GetAdminTagsUseCase : IGetAdminTagsUseCase
    {
        private readonly ITagRepository _tagRepository;

        public GetAdminTagsUseCase(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IReadOnlyList<TagResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return await _tagRepository.GetAllAsync(ct);
        }
    }
}
