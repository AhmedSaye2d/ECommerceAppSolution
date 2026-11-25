using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Domain.Entities
{
    /// <summary>
    /// Represents a catalog product available for purchase in the e-commerce store.
    /// </summary>
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string Image { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public Category? Category { get; set; }

        public Guid CategoryId { get; set; }
    }
}
