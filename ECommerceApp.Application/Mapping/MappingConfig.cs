using AutoMapper;
using ECommerceApp.Application.Dto.Category;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Domain.Entities;
using System;
using System.Data.Common;
namespace ECommerceApp.Application.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateCategory,Category>();
            CreateMap<CreateProduct, Product>();
            CreateMap<Category, GetCategory>();
            CreateMap<Product, GetProduct>();
        }
    }
}
