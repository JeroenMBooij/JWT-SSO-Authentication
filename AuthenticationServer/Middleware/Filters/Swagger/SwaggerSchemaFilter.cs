using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace AuthenticationServer.Web.Middleware.Filters.Swagger
{
    public class SwaggerSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var keys = new List<string>();
            List<string> filter = new List<string>();
            filter.Add("JsonValidator");
            filter.Add("JSchemaType");
            filter.Add("JToken");
            filter.Add("JSchema");

            foreach (var key in context.SchemaRepository.Schemas.Keys)
            {
                if (filter.Contains(key))
                {
                    keys.Add(key);
                }
            }
            foreach (var key in keys)
            {
                context.SchemaRepository.Schemas.Remove(key);
            }
        }
    }

}
