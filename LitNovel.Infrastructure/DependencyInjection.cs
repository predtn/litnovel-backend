using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Infrastructure.Persistences;
using LitNovel.Infrastructure.Persistences.Repositories;
using LitNovel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LitNovel.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MyCnn");

            services.AddDbContext<LitNovelContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<INovelRepository, NovelRepository>();
            services.AddScoped<IVolumeRepository, VolumeRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IUserReportRepository, UserReportRepository>();
            services.AddScoped<IReadingProgressRepository, ReadingProgressRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<INovelRatingRepository, NovelRatingRepository>();
            services.AddScoped<ICommentChapterRepository, CommentChapterRepository>();
            services.AddScoped<INovelReportRepository, NovelReportRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INovelLikeRepository, NovelLikeRepository>();
            services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();
            services.AddScoped<IAdminStatisticsRepository, AdminStatisticsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordResetTokenService, PasswordResetTokenService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
