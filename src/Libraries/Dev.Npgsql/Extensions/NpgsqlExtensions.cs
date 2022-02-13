﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Npgsql.Extensions
{
    public static class NpgsqlExtensions
    {
        public static IServiceCollection AddNpgsqlConfig<TContext>(this IServiceCollection services,
            IConfiguration configuration) where TContext : DbContext
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TContext>(options => { options.UseNpgsql(connection); });
            services.AddScoped<DbContext, TContext>();
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IApplicationBuilder AddMigrateConfigure<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();
                context.Database.EnsureCreated();
            }

            return app;
        }

        /// <summary>  
        /// Migrates the database.  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="host">The web host.</param>  
        /// <returns>IWebHost.</returns>  
        public static IHost CreateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            try
            {
                var context = services.GetRequiredService<TContext>();
                context.Database.EnsureCreated();
                logger.LogInformation("Database migration completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred migrate the DB.");
            }

            return host;
        }
    }
}
