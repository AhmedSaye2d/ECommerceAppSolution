using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.Dto.Category
{
    public class CategoryBase
    {
        [Required]
        public string? Name { get; set; }
    }

}
