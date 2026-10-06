using ECommerceApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApp.Domain.Interface
{
    public interface IProductRepository : IGeneric<Product>
    {
        Task<(IEnumerable<Product> Items, int TotalCount)> GetProductsAsync(
            string? search, 
            Guid? categoryId, 
            decimal? minPrice, 
            decimal? maxPrice, 
            string? sortBy, 
            int pageNumber, 
            int pageSize);
    }
}
