using AutoMapper;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interface;
using System;

namespace ECommerceApp.Application.Services.Implementation
{
    public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
    {
        public async Task<ServiceResponse> AddAsync(CreateProduct product)
        {
            var mappData = mapper.Map<Product>(product);

            var result = await productRepository.AddAsync(mappData);

            return result > 0 ? new ServiceResponse(true, "Product added successfully")

               : new ServiceResponse(false, "Product failed to be added");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var result = await productRepository.DeleteAsync(id);

            return result > 0 ? new ServiceResponse(true, "Product deleted successfully") 

                : new ServiceResponse(false, "Product failed to be deleted");

        }

        public async Task<IEnumerable<GetProduct>> GetAllAsync()
        {
           var rawdata= await productRepository.GetAllAsync();
            if (!rawdata.Any()) return [];

           return mapper.Map<IEnumerable<GetProduct>>(rawdata);

        }

        public async Task<GetProduct> GetByIdAsync(Guid id)
        {
            var rawdata = await productRepository.GetByIdAsync(id);
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
            var result = await productRepository.UpdateAsync(mappData);
            return result > 0 ? new ServiceResponse(true, "Product updated successfully")
               : new ServiceResponse(false, "Product failed to be updated");
        }

        public async Task<PaginatedResult<GetProduct>> GetProductsAsync(GetProductsQueryDto query)
        {
            var (items, totalCount) = await productRepository.GetProductsAsync(
                query.Search,
                query.CategoryId,
                query.MinPrice,
                query.MaxPrice,
                query.SortBy,
                query.PageNumber,
                query.PageSize);

            return new PaginatedResult<GetProduct>
            {
                Items = mapper.Map<IEnumerable<GetProduct>>(items),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
