using System.Text;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.WebAPI.Common;
using LitNovel.WebAPI.Common.Json;
using LitNovel.WebAPI.Configs;
using LitNovel.WebAPI.Middlewares;
using LitNovel.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace LitNovel.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPI(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new NullableDateTimeJsonConverter());
                })
                .AddOData(options => options
                    .Filter()
                    .OrderBy()
                    .Count()
                    .SetMaxTop(100));
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LitNovel API",
                    Version = "v1",
                    Description = "Quick testing surface for LitNovel backend APIs."
                });
                options.OperationFilter<ODataQueryOptionsOperationFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste only the JWT access token returned by /api/auth/login."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document, null),
                        new List<string>()
                    }
                });
            });
            services.AddScoped<ExceptionHandlingMiddleware>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddOptions<JwtConfig>()
                .BindConfiguration("Jwt")
                .ValidateDataAnnotations();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            services.AddAuthorization();
            services.ConfigureOptions<JwtBearerOptionsSetup>();

            return services;
        }
    }

    internal class JwtBearerOptionsSetup : Microsoft.Extensions.Options.IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtBearerOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name != JwtBearerDefaults.AuthenticationScheme)
            {
                return;
            }

            var key = _configuration["Jwt:Key"] ?? "LitNovel development signing key with at least thirty two chars";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = !string.IsNullOrWhiteSpace(_configuration["Jwt:Issuer"]),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = !string.IsNullOrWhiteSpace(_configuration["Jwt:Audience"]),
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
        }
    }
}
