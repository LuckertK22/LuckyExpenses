using LuckyExpenses.Shared.Common;
using LuckyExpenses.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                .AddJwtBearer(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;

                    if (string.IsNullOrWhiteSpace(jwtOptions.Key))
                        throw new InvalidOperationException("Jwt:Key no configurado");

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
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
                            context.Response.ContentType = "application/json";

                            return context.Response.WriteAsync(
                                JsonSerializer.Serialize(new ApiResponse<object>
                                {
                                    Ok = false,
                                    Message = "Token invalido o no proporcionado",
                                    Data = null
                                }));
                        }
                    };
                });

            return services;
        }
    }
}
