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
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
            services.AddScoped<IGetNovelsUseCase, GetNovelsUseCase>();
            services.AddScoped<ICreateUserReportUseCase, CreateUserReportUseCase>();

            return services;
        }
    }
}
