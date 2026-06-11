using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LitNovel.WebAPI.Common
{
    public class ODataQueryOptionsOperationFilter : IOperationFilter
    {
        private static readonly string[] ODataParameters =
        [
            "$filter",
            "$orderby",
            "$skip",
            "$top",
            "$count"
        ];

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasODataQueryOptions = context.MethodInfo.GetParameters()
                .Any(parameter => IsODataQueryOptions(parameter.ParameterType));

            if (!hasODataQueryOptions)
            {
                return;
            }

            operation.Parameters ??= [];

            var generatedQueryOptions = operation.Parameters
                .Where(parameter => parameter.Name?.Contains("queryOptions", StringComparison.OrdinalIgnoreCase) == true)
                .ToList();

            foreach (var parameter in generatedQueryOptions)
            {
                operation.Parameters.Remove(parameter);
            }

            foreach (var parameterName in ODataParameters)
            {
                if (operation.Parameters.Any(parameter => parameter.Name == parameterName))
                {
                    continue;
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = parameterName,
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = parameterName is "$skip" or "$top"
                            ? JsonSchemaType.Integer
                            : JsonSchemaType.String
                    }
                });
            }
        }

        private static bool IsODataQueryOptions(Type type)
        {
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>);
        }
    }
}
