using LitNovel.WebAPI.Middlewares;

namespace LitNovel.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPI(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddScoped<ExceptionHandlingMiddleware>();

            return services;
        }
    }
}
