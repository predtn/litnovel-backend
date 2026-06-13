using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Tag;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class UpdateAdminTagUseCase : IUpdateAdminTagUseCase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTagRequestDto> _validator;

        public UpdateAdminTagUseCase(
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateTagRequestDto> validator)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TagResponseDto> ExecuteAsync(int id, UpdateTagRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var tag = await _tagRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Tag not found");

            if (request.Name is not null)
            {
                var name = request.Name.Trim();
                if (await _tagRepository.NameExistsAsync(name, id, ct))
                {
                    throw new ConflictException("Tag name already exists");
                }

                tag.Name = name;
            }

            if (request.Slug is not null)
            {
                var slug = request.Slug.Trim();
                if (await _tagRepository.SlugExistsAsync(slug, id, ct))
                {
                    throw new ConflictException("Slug already exists");
                }

                tag.Slug = slug;
            }

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
