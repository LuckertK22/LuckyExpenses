using LuckyExpenses.Shared.Options;

namespace LuckyExpenses.WebAPI.Config.Options
{
    public static class ConfigureOptions
    {
        public static IServiceCollection AddConfiguredOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            return services;
        }
    }
}
