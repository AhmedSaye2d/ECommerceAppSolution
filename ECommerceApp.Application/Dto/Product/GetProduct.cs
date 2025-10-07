using ECommerceApp.Application.Dto.Category;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.Dto.Product
{
    public class GetProduct:ProductBase
    {
        [Required]
        public Guid Id { get; set; }
        public GetCategory? Category { get; set; }
    }

}
