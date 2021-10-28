using Dev.Framework.Security.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Framework.Extensions
{
    public static class CrosExtensions
    {
        public static IServiceCollection AddAdminApiCors(this IServiceCollection services, ApiTokenOptions apiTokenOptions)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        if (apiTokenOptions.CorsAllowAnyOrigin)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else
                        {
                            builder.WithOrigins(apiTokenOptions.CorsAllowOrigins);
                        }

                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}