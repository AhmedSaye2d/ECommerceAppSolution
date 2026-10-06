using AutoMapper;
using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Category;
using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Dto.Order;
using ECommerceApp.Application.Services.Implementation.Cart;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Entities.Order;
using System;
using System.Data.Common;
namespace ECommerceApp.Application.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateCategory, Category>();
            CreateMap<UpdateCategory, Category>();
            CreateMap<CreateProduct, Product>();
            CreateMap<UpdateProduct, Product>();
            CreateMap<Category, GetCategory>();
            CreateMap<Product, GetProduct>();
            CreateMap<CreateUser, AppUser>();
            CreateMap<LoginUser, AppUser>();
            CreateMap<PaymentMethod, GetPaymentMethod>();
            CreateMap<CreateAchieve, Achieve>();
            CreateMap<Order, GetOrderDto>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<OrderItem, GetOrderItemDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
        }
    }
}
