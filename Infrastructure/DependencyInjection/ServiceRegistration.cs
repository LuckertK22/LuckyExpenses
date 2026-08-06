using LuckyExpenses.Application.Context;
using LuckyExpenses.Application.Interfaces.Authentication;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Authentication;
using LuckyExpenses.Infrastructure.Context;
using LuckyExpenses.Infrastructure.Persistence;
using LuckyExpenses.Infrastructure.Repositories;
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

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IHasherService, HasherService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}