using E_commerce.Domain.Interfaces;
using E_commerce.Infrastructure.Repositories;
using E_commerce.Infrastructure.Seeding;
using E_commerce.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //repositorios
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPasswordEncryptionService, PasswordEncryptionService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();

            // registramos el DataSeeder
            services.AddTransient<DataSeeder>();


            return services;
        }
    }
}
