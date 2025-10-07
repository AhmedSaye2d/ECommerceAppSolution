using AutoMapper;
using ECommerceApp.Application.Dto.Category;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interface;
namespace ECommerceApp.Application.Services.Implementation
{
    public class CategoryService(IGeneric<Category> CategoryInterface,IMapper mapper): ICategoryService
    {
        public async Task<ServiceResponse> AddAsync(CreateCategory category)
        {
            var mapdata=mapper.Map<Category>(category);
            var result = await CategoryInterface.AddAsync(mapdata);
                return result > 0 ? new ServiceResponse(true, "Category in Delete") :
                new ServiceResponse(false, "Category Falied to be Delete");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var result = await CategoryInterface.DeleteAsync(id);
            return result > 0 ? new ServiceResponse(true, "Category in Delete") :
                new ServiceResponse(false, "Category Falied to be Delete");

        }

        public async Task<IEnumerable<GetCategory>> GetAllAsync()
        {
          var rawdata=await CategoryInterface.GetAllAsync();
            if (!rawdata.Any()) return [];
            return mapper.Map<IEnumerable<GetCategory>>(rawdata);
        }

        public async Task<GetCategory> GetByIdAsync(Guid id)
        {
           var rawdata =await CategoryInterface.GetByIdAsync(id);
            if (rawdata == null)
            {
                return new GetCategory();
            }
            else
            {
                return mapper.Map<GetCategory>(rawdata);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateCategory category)
        {
            var mappData = mapper.Map<Category>(category);
            var result = await CategoryInterface.UpdateAsync(mappData); 
            return result > 0 ? new ServiceResponse(true, "Category update")
               : new ServiceResponse(false, "Category Falied to be update");
        }
    }
}
