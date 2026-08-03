using LuckyExpenses.Application.Interfaces.Authentication;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Domain.Services;
using LuckyExpenses.Infrastructure.Authentication;
using LuckyExpenses.Infrastructure.Persistence;
using LuckyExpenses.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(
                    configuration.GetConnectionString("DefaultConnection"));
                var dataSource = dataSourceBuilder.Build();
                options.UseNpgsql(dataSource);
            });

            services.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                })
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();

            AddRepositories(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var repoAssembly = typeof(UnitOfWork).Assembly;
            var repoTypes = repoAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"));

            foreach (var impl in repoTypes)
            {
                var iface = impl.GetInterfaces().FirstOrDefault(i => i.Name == "I" + impl.Name);
                if (iface != null)
                    services.AddScoped(iface, impl);
            }
        }
    }
}