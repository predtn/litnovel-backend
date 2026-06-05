using FluentValidation;
using LitNovel.WebAPI.Common.Models;

namespace LitNovel.WebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = GetStatusCode(exception);

                await context.Response.WriteAsJsonAsync(new ApiResponse<object>
                {
                    Success = false,
                    Message = exception.Message,
                    Data = null
                });
            }
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                _ when exception.GetType().Name == "BadRequestException" => StatusCodes.Status400BadRequest,
                _ when exception.GetType().Name == "UnauthorizedException" => StatusCodes.Status401Unauthorized,
                _ when exception.GetType().Name == "ForbiddenException" => StatusCodes.Status403Forbidden,
                _ when exception.GetType().Name == "NotFoundException" => StatusCodes.Status404NotFound,
                _ when exception.GetType().Name == "ConflictException" => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
