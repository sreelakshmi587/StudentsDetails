using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StudentsDetails.Infrastructure.ActionFilters
{
    public class SwaggerCustomFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes().OfType<SwaggerOperationAttribute>().Any())
            {
                if (context.MethodInfo.Name == "Login")
                {
                    var schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { "UserName", new OpenApiSchema { Type = "string" } },
                        { "Password", new OpenApiSchema { Type = "string" } }
                    }
                    };

                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "application/json", new OpenApiMediaType
                            {
                                Schema = schema
                            }
                        }
                    }
                    };
                }

                if (context.MethodInfo.Name == "Register")
                {
                    var schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { "UserName", new OpenApiSchema { Type = "string" } },
                        { "Password", new OpenApiSchema { Type = "string" } },
                        { "Email", new OpenApiSchema { Type = "string" } },
                        { "Role", new OpenApiSchema { Type = "string" } }
                    }
                    };

                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "application/json", new OpenApiMediaType
                            {
                                Schema = schema
                            }
                        }
                    }
                    };
                }
            }
        }
    }
}
