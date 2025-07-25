using E_commerce.Validatos;
using FluentValidation;

namespace E_commerce.Extensions
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            services.AddValidatorsFromAssemblyContaining<ProductQueryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<OrderItemRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ChangeStatusDtoValidator>();





            return services;
        }
    }
}
