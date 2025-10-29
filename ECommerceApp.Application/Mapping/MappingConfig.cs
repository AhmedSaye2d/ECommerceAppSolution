using AutoMapper;
using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Category;
using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Implementation.Cart;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Entities.Identity;
using System;
using System.Data.Common;
namespace ECommerceApp.Application.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateCategory, Category>();
            CreateMap<CreateProduct, Product>();
            CreateMap<Category, GetCategory>();
            CreateMap<Product, GetProduct>();
            CreateMap<CreateUser, AppUser>();
            CreateMap<LoginUser, AppUser>();
            CreateMap<PaymentMethod, GetPaymentMethod>();
            CreateMap<CreateAchieve, Achieve>();
        }
    }
}
