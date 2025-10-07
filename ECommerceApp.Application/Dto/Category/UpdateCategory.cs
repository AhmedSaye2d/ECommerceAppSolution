using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.Dto.Category
{
    public class UpdateCategory : CategoryBase
    {
        [Required]
        public Guid Id { get; set; }
    }

}
