using Dev.Core.IoC;
using Dev.Framework.Exceptions;
using Dev.Framework.Extensions;
using Dev.Framework.Helper.ModelStateResponseFactory;
using Dev.Framework.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Dev.Framework.Security.Model;

namespace Dev.Framework.Systems
{
    public class BaseStartup
    {
        protected string SwaggerTitle = "My API";
        protected string SwaggerVersion = "v1";
        protected ApiTokenOptions TokenOptions;

        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
            SwaggerTitle = Configuration.GetSection("SwaggerTitle")?.Value;
            SwaggerVersion = $"v{Configuration.GetSection("ApiVersion:MajorVersion")?.Value}";
        }

        protected IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var tokenOptionsConfiguration = Configuration.GetSection("TokenOptions");

            services.Configure<ApiTokenOptions>(tokenOptionsConfiguration);

            TokenOptions = tokenOptionsConfiguration.Get<ApiTokenOptions>();

            services.AddControllers().AddJsonOptionsConfig();

            services.AddAdminApiCors(TokenOptions);

            services.AddApiVersioningConfig(Configuration);

            services.AddHttpContextAccessor();

            services.AddSwaggerGenConfig(TokenOptions);

            services.RegisterAll<IService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<ApiBehaviorOptions>(options => { options.InvalidModelStateResponseFactory = ctx => new ModelStateFeatureFilter(); });
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    if (context.Request.IsLocal())
                    {
                        // Forbidden http status code
                        context.Response.StatusCode = 403;
                        return;
                    }

                    await next.Invoke();
                });
            }

            app.UseStaticFiles();

            app.UseSwaggerUIConfig(TokenOptions);

            app.UseRouting();

            app.UseCorsConfig();

            app.UseMiddleware<ResponseRewindMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseStatusCodePages(new StatusCodePagesOptions()
            {
                HandleAsync = (ctx) =>
                {
                    if (ctx.HttpContext.Response.StatusCode == 404)
                    {
                        throw new NotFoundException($"Not Found Page");
                    }

                    return Task.FromResult(0);
                }
            });

            app.ConfigureRequestPipeline();
        }
    }
}