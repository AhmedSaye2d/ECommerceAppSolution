using AutoMapper;
using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Category;
using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Dto.Order;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Entities.Order;

namespace ECommerceApp.Application.Mapping
{
    /// <summary>
    /// AutoMapper profile configuring clean bi-directional mappings between Domain entities and DTOs.
    /// </summary>
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Category Mappings
            CreateMap<CreateCategory, Category>();
            CreateMap<UpdateCategory, Category>();
            CreateMap<Category, GetCategory>();

            // Product Mappings
            CreateMap<CreateProduct, Product>();
            CreateMap<UpdateProduct, Product>();
            CreateMap<Product, GetProduct>();

            // Identity Mappings
            CreateMap<CreateUser, AppUser>();
            CreateMap<LoginUser, AppUser>();

            // Cart & Payment Mappings
            CreateMap<PaymentMethod, GetPaymentMethod>();
            CreateMap<CreateAchieve, Achieve>();

            // Order & OrderItem Projections
            CreateMap<Order, GetOrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OrderItem, GetOrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
        }
    }
}
