using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
namespace ECommerce.Infrastructure.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options):base(options) 
        {
                
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Achieve> CheckoutAchieves {  get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PaymentMethod>()
               .HasData(
               new PaymentMethod
               {
                   Id = Guid.NewGuid(),
                   Name = "CreditCard",
               });
              
            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                });
                
        }
    }
}
