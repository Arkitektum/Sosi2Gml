using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sosi2Gml.Web.Configuration
{
    public class MultipartOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var mediaType = new OpenApiMediaType()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties =
                    {
                        ["sosiFile"] = new OpenApiSchema
                        {
                            Type = "file",
                            Format = "binary"
                        }
                    },
                    Required = new HashSet<string>() { "sosiFile" }
                }
            };
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = mediaType }
            };
        }
    }
}
