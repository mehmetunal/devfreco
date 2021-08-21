using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Framework.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            var tokenOptionsConfiguration = configuration.GetSection("TokenOptions");

            services.Configure<ApiTokenOptions>(tokenOptionsConfiguration);

            var tokenOptions = tokenOptionsConfiguration.Get<ApiTokenOptions>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Gelen isteğin doğru siteden olduğunu kontrol eder,
                        ValidateAudience = false,
                        //Gelen isteğin doğru siteden olduğunu kontrol eder, //Bu iki ayar ise "aud" ve "iss" claimlerini kontrol edelim mi diye soruyor
                        ValidateIssuer = false,
                        //Gelen her tokenun doğrulankasını sağlıyor.Token 3.kısım(imza) kontrolü
                        ValidateIssuerSigningKey = true,
                        //Doğrulama Keyini Tanımladığımız yer.Neyle kontrol etmesi gerektigi
                        IssuerSigningKey = SingHandler.GetSecurityKey(tokenOptions.SecurityKey),

                        ClockSkew = TimeSpan.Zero,
                        //Süresi dolmamış token vermesini sağlıyor
                        ValidateLifetime = true,
                    };
                    x.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = (context) =>
                        {
                            Console.WriteLine(context.Exception);
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.WriteAsync("401 Unauthorized");
                            throw new UnauthorizedAccessException();
                            return Task.CompletedTask;
                        },
                        OnForbidden = (context) =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            throw new Exception("Forbidden");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = (context) =>
                        {
                            context.Request.Headers.TryGetValue("Authorization", out var BearerToken);
                            if (BearerToken.Count == 0)
                                throw new UnauthorizedAccessException();

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = (context) => { return Task.CompletedTask; },
                        OnChallenge = (context) => { return Task.CompletedTask; }
                    };
                });
            return services;
        }
    }
}
