using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Tag;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateAdminTagUseCase : ICreateAdminTagUseCase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTagRequestDto> _validator;

        public CreateAdminTagUseCase(
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateTagRequestDto> validator)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TagResponseDto> ExecuteAsync(CreateTagRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var name = request.Name.Trim();
            var slug = request.Slug.Trim();

            if (await _tagRepository.NameExistsAsync(name, excludingId: null, ct))
            {
                throw new ConflictException("Tag name already exists");
            }

            if (await _tagRepository.SlugExistsAsync(slug, excludingId: null, ct))
            {
                throw new ConflictException("Slug already exists");
            }

            var tag = new Tag
            {
                Name = name,
                Slug = slug
            };

            await _tagRepository.AddAsync(tag, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Map(tag);
        }

        private static TagResponseDto Map(Tag tag)
        {
            return new TagResponseDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug
            };
        }
    }
}
