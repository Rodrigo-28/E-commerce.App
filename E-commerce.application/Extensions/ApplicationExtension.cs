using E_commerce.application.Interfaces;
using E_commerce.application.Services;
using E_commerce.Domain.Common;
using E_commerce.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce.application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            {
                services.AddTransient<IProductService, ProductService>();
                services.AddTransient<IOrderService, OrderService>();
                services.AddTransient<ICategoryService, CategoryService>();
                services.AddTransient<IUserService, UserService>();

                services.AddTransient<IAuthService, AuthService>();
                services.AddSingleton<IOrderQueue, OrderQueue>();







                return services;
            }
        }
    }
}
