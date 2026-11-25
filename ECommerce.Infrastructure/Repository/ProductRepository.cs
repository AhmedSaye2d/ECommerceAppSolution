using ECommerce.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repository
{
    /// <summary>
    /// Specialized repository implementing optimized catalog discovery with deferred LINQ execution.
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetProductsAsync(
            string? search, 
            Guid? categoryId, 
            decimal? minPrice, 
            decimal? maxPrice, 
            string? sortBy, 
            int pageNumber, 
            int pageSize)
        {
            // Base read-only query with eager category inclusion
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsQueryable();

            // 1. Dynamic Category Filtering
            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // 2. Range Price Filtering
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // 3. Full-Text Search in Title and Description
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(p => (p.Name != null && p.Name.ToLower().Contains(lowerSearch)) 
                                      || (p.Description != null && p.Description.ToLower().Contains(lowerSearch)));
            }

            // Count total matching items before pagination
            int totalCount = await query.CountAsync();

            // 4. Dynamic Sorting
            query = sortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "name_asc" => query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            // 5. Server-Side Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
