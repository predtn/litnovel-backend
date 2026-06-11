using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.UseCases
{
    public class GetTagsUseCase : IGetTagsUseCase
    {
        private readonly ITagRepository _tagRepository;

        public GetTagsUseCase(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public Task<List<TagResponseDto>> ExecuteAsync(CancellationToken ct)
        {
            return _tagRepository.GetAllAsync(ct);
        }
    }
}
