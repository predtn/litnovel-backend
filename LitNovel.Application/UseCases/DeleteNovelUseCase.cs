using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class DeleteNovelUseCase : IDeleteNovelUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNovelUseCase(INovelRepository novelRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var novel = await _novelRepository.GetByIdForUpdateAsync(id, ct);
            if (novel == null)
            {
                throw new NotFoundException("Novel not found");
            }

            if (!CanManage(novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to delete this novel");
            }

            _novelRepository.Delete(novel);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private bool CanManage(int authorId)
        {
            return authorId == _currentUserService.UserId
                || string.Equals(_currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(_currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
