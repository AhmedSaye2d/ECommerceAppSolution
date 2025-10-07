using AutoMapper;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interface;
using System;

namespace ECommerceApp.Application.Services.Implementation
{
    public class ProductService(IGeneric<Product> ProductInterface, IMapper mapper) : IproductService
    {
        public async Task<ServiceResponse> AddAsync(CreateProduct product)
        {
           var mappData=mapper.Map<Product>(product);
            var result = await ProductInterface.AddAsync(mappData);
            return result > 0 ? new ServiceResponse(true, "Product Add")
               : new ServiceResponse(false, "Produt Falied to be Add");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var result = await ProductInterface.DeleteAsync(id);
            return result > 0 ? new ServiceResponse(true, "Product Delete") 
                : new ServiceResponse(false, "Produt Falied to be Delete");

        }

        public async Task<IEnumerable<GetProduct>> GetAllAsync()
        {
           var rawdata= await ProductInterface.GetAllAsync();
            if (!rawdata.Any()) return [];

           return mapper.Map<IEnumerable<GetProduct>>(rawdata);

        }

        public async Task<GetProduct> GetByIdAsync(Guid id)
        {
            var rawdata = await ProductInterface.GetByIdAsync(id);
            if (rawdata == null)
            {
                return new GetProduct();
            }
            else
            {
                return mapper.Map<GetProduct>(rawdata);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateProduct product)
        {
            var mappData = mapper.Map<Product>(product);
            var result = await ProductInterface.UpdateAsync(mappData);
            return result > 0 ? new ServiceResponse(true, "Product update")
               : new ServiceResponse(false, "Produt Falied to be update");
        }
    }
}
