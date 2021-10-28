using System.Collections.Generic;
using System.Linq;
using Dev.Framework.Security.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dev.Framework.Security.Authorization
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly ApiTokenOptions _apiTokenOptions;

        public AuthorizeCheckOperationFilter(ApiTokenOptions apiTokenOptions)
        {
            _apiTokenOptions = apiTokenOptions;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType != null && (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                                                            || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse {Description = "Unauthorized"});
                operation.Responses.Add("403", new OpenApiResponse {Description = "Forbidden"});

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            }
                        ] = new[] {_apiTokenOptions.OidcApiName}
                    }
                };
            }
        }
    }
}