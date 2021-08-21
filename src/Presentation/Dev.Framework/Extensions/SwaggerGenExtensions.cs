using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Dev.Framework.Extensions
{
    public static class SwaggerGenExtensions
    {
        /// <summary>
        /// AddSwaggerGenConfig
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerGenConfig(this IServiceCollection services, string Title = "My API", string Version = "v1")
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc(Version, new OpenApiInfo { Title = Title, Version = Version }); });
            return services;
        }

        public static IApplicationBuilder UseSwaggerUIConfig(this IApplicationBuilder app, string Title = "My APi")
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Title);
                c.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
