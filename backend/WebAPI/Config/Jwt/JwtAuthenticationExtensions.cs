using LuckyExpenses.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LuckyExpenses.WebAPI.Config.Jwt
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtOptions>>((options, jwtOptionsAccessor) =>
                {
                    var jwtOptions = jwtOptionsAccessor.Value;

                    if (string.IsNullOrWhiteSpace(jwtOptions.Key))
                        throw new InvalidOperationException("Jwt:Key no configurado");

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                            var problemDetails = new ProblemDetails
                            {
                                Type = "about:blank",
                                Status = (int)HttpStatusCode.Unauthorized,
                                Title = "No autorizado",
                                Detail = "Token inválido o no proporcionado",
                                Instance = context.Request.Path
                            };

                            context.Response.ContentType = "application/problem+json";
                            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, JsonSerializerOptions.Web));
                        }
                    };
                });

            return services;
        }
    }
}
