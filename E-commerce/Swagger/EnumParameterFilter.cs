using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace E_commerce.Swagger
{
    public class EnumParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var type = context.ParameterInfo?.ParameterType;
            if (type != null && type.IsEnum)
            {
                // Hacemos que Swagger trate este parámetro como string
                parameter.Schema.Type = "string";
                parameter.Schema.Format = null;

                // Rellenamos el enum con los nombres de las constantes
                parameter.Schema.Enum = Enum
                    .GetNames(type)
                    .Select(name => new OpenApiString(name))
                    .Cast<IOpenApiAny>()
                    .ToList();
            }
        }
    }
}
