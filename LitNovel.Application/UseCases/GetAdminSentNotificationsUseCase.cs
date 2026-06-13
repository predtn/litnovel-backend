using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.UseCases
{
    public class GetAdminSentNotificationsUseCase : IGetAdminSentNotificationsUseCase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IValidator<AdminSentNotificationsQueryDto> _validator;

        public GetAdminSentNotificationsUseCase(
            INotificationRepository notificationRepository,
            IValidator<AdminSentNotificationsQueryDto> validator)
        {
            _notificationRepository = notificationRepository;
            _validator = validator;
        }

        public async Task<PagedResult<AdminSentNotificationResponseDto>> ExecuteAsync(AdminSentNotificationsQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _notificationRepository.GetAdminSentAsync(query, ct);
        }
    }
}
