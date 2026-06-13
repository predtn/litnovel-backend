using FluentValidation;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace LitNovel.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddScoped<IRegisterUseCase, RegisterUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<ILogoutUseCase, LogoutUseCase>();
            services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
            services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
            services.AddScoped<IGetMyProfileUseCase, GetMyProfileUseCase>();
            services.AddScoped<IUpdateMyProfileUseCase, UpdateMyProfileUseCase>();
            services.AddScoped<IGetPublicProfileUseCase, GetPublicProfileUseCase>();
            services.AddScoped<ISearchUsersUseCase, SearchUsersUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
            services.AddScoped<IGetTagsUseCase, GetTagsUseCase>();
            services.AddScoped<IGetNovelsUseCase, GetNovelsUseCase>();
            services.AddScoped<IGetNovelUseCase, GetNovelUseCase>();
            services.AddScoped<IGetNovelAnalyticsUseCase, GetNovelAnalyticsUseCase>();
            services.AddScoped<IGetMyNovelsUseCase, GetMyNovelsUseCase>();
            services.AddScoped<ICreateNovelUseCase, CreateNovelUseCase>();
            services.AddScoped<IUpdateNovelUseCase, UpdateNovelUseCase>();
            services.AddScoped<ISubmitNovelUseCase, SubmitNovelUseCase>();
            services.AddScoped<IDeleteNovelUseCase, DeleteNovelUseCase>();
            services.AddScoped<IGetVolumesUseCase, GetVolumesUseCase>();
            services.AddScoped<ICreateVolumeUseCase, CreateVolumeUseCase>();
            services.AddScoped<IUpdateVolumeUseCase, UpdateVolumeUseCase>();
            services.AddScoped<IDeleteVolumeUseCase, DeleteVolumeUseCase>();
            services.AddScoped<IGetChaptersUseCase, GetChaptersUseCase>();
            services.AddScoped<IGetChapterUseCase, GetChapterUseCase>();
            services.AddScoped<ICreateChapterUseCase, CreateChapterUseCase>();
            services.AddScoped<IUpdateChapterUseCase, UpdateChapterUseCase>();
            services.AddScoped<ISubmitChapterUseCase, SubmitChapterUseCase>();
            services.AddScoped<IDeleteChapterUseCase, DeleteChapterUseCase>();
            services.AddScoped<ICreateUserReportUseCase, CreateUserReportUseCase>();
            services.AddScoped<IGetReadingHistoryUseCase, GetReadingHistoryUseCase>();
            services.AddScoped<IGetNotificationsUseCase, GetNotificationsUseCase>();
            services.AddScoped<ISaveReadingProgressUseCase, SaveReadingProgressUseCase>();
            services.AddScoped<IDeleteReadingHistoryUseCase, DeleteReadingHistoryUseCase>();
            services.AddScoped<IGetMyFavoritesUseCase, GetMyFavoritesUseCase>();
            services.AddScoped<IAddFavoriteUseCase, AddFavoriteUseCase>();
            services.AddScoped<IRemoveFavoriteUseCase, RemoveFavoriteUseCase>();
            services.AddScoped<IGetMyNovelLikesUseCase, GetMyNovelLikesUseCase>();
            services.AddScoped<IAddNovelLikeUseCase, AddNovelLikeUseCase>();
            services.AddScoped<IRemoveNovelLikeUseCase, RemoveNovelLikeUseCase>();
            services.AddScoped<IGetNovelReviewsUseCase, GetNovelReviewsUseCase>();
            services.AddScoped<ICreateNovelReviewUseCase, CreateNovelReviewUseCase>();
            services.AddScoped<IUpdateNovelReviewUseCase, UpdateNovelReviewUseCase>();
            services.AddScoped<IDeleteNovelReviewUseCase, DeleteNovelReviewUseCase>();
            services.AddScoped<IGetChapterCommentsUseCase, GetChapterCommentsUseCase>();
            services.AddScoped<ICreateChapterCommentUseCase, CreateChapterCommentUseCase>();
            services.AddScoped<ICreateCommentReplyUseCase, CreateCommentReplyUseCase>();
            services.AddScoped<IUpdateCommentUseCase, UpdateCommentUseCase>();
            services.AddScoped<IDeleteCommentUseCase, DeleteCommentUseCase>();
            services.AddScoped<IAddCommentLikeUseCase, AddCommentLikeUseCase>();
            services.AddScoped<IRemoveCommentLikeUseCase, RemoveCommentLikeUseCase>();
            services.AddScoped<ICreateNovelReportUseCase, CreateNovelReportUseCase>();
            services.AddScoped<IGetAdminStatisticsUseCase, GetAdminStatisticsUseCase>();
            services.AddScoped<IGetAdminUsersUseCase, GetAdminUsersUseCase>();
            services.AddScoped<IGetAdminUserDetailUseCase, GetAdminUserDetailUseCase>();
            services.AddScoped<IUpdateAdminUserUseCase, UpdateAdminUserUseCase>();
            services.AddScoped<IBanAdminUserUseCase, BanAdminUserUseCase>();
            services.AddScoped<IUnbanAdminUserUseCase, UnbanAdminUserUseCase>();
            services.AddScoped<IDeleteAdminUserUseCase, DeleteAdminUserUseCase>();
            services.AddScoped<IAssignStaffUseCase, AssignStaffUseCase>();
            services.AddScoped<IRevokeStaffUseCase, RevokeStaffUseCase>();
            services.AddScoped<IGetAdminBadgesUseCase, GetAdminBadgesUseCase>();
            services.AddScoped<ICreateAdminBadgeUseCase, CreateAdminBadgeUseCase>();
            services.AddScoped<IUpdateAdminBadgeUseCase, UpdateAdminBadgeUseCase>();
            services.AddScoped<IDeleteAdminBadgeUseCase, DeleteAdminBadgeUseCase>();
            services.AddScoped<IAwardBadgeUseCase, AwardBadgeUseCase>();
            services.AddScoped<IGetAdminCategoriesUseCase, GetAdminCategoriesUseCase>();
            services.AddScoped<ICreateAdminCategoryUseCase, CreateAdminCategoryUseCase>();
            services.AddScoped<IUpdateAdminCategoryUseCase, UpdateAdminCategoryUseCase>();
            services.AddScoped<IDeleteAdminCategoryUseCase, DeleteAdminCategoryUseCase>();
            services.AddScoped<IGetAdminTagsUseCase, GetAdminTagsUseCase>();
            services.AddScoped<ICreateAdminTagUseCase, CreateAdminTagUseCase>();
            services.AddScoped<IUpdateAdminTagUseCase, UpdateAdminTagUseCase>();
            services.AddScoped<IDeleteAdminTagUseCase, DeleteAdminTagUseCase>();
            services.AddScoped<IGetAdminSentNotificationsUseCase, GetAdminSentNotificationsUseCase>();
            services.AddScoped<ISendAdminNotificationUseCase, SendAdminNotificationUseCase>();

            return services;
        }
    }
}
