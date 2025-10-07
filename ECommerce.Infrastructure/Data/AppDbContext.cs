using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
namespace ECommerce.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
