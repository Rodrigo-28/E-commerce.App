using AutoMapper;
using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.Domain.Models;

namespace E_commerce.application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // ----- PRODUCTOS -----
            // Entidad → DTO (incluye CategoryName)
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name));

            // DTO de creación → Entidad
            CreateMap<CreateProductDto, Product>();

            // ----- CATEGORÍAS -----
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CreateCategoryDto, Category>();

            // ----- ITEMS DE ORDEN -----
            // Entidad → DTO (incluye ProductName)
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName,
                           opt => opt.MapFrom(src => src.Product.Name));

            // DTO de request → Entidad (ignora relación a Order y PK)
            CreateMap<OrderItemRequest, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // ----- ÓRDENES -----
            // Entidad → DTO (incluye lista de items y calcula TotalAmount)
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalAmount,
                           opt => opt.MapFrom(src => src.GetTotalAmount()))
                .ForMember(dest => dest.Items,
                           opt => opt.MapFrom(src => src.Items));

            // DTO de creación → Entidad (ignora campos que se generan en el servicio)
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());


            // Entidad → DTO de respuesta
            CreateMap<User, UserResponseDto>();

            // Opcional: si quieres mapear también CreateUserDto/UpdateUserDto → User
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
