using ECommerceApp.Application.Dto.Category;
using ECommerceApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService IcategoryService;
        public CategoryController(ICategoryService _IcategoryService)
        {
            this.IcategoryService = _IcategoryService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await IcategoryService.GetAllAsync();
            return data.Any() ? Ok(data) : NotFound(data);
        }
        [HttpGet("Single/{id}")]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            var data = await IcategoryService.GetByIdAsync(id);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound(data);
            }
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateCategory category)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);  
            var result = await IcategoryService.AddAsync(category);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory(UpdateCategory category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result =await IcategoryService.UpdateAsync(category);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCateroy(Guid id)
        {
            var result = await IcategoryService.DeleteAsync(id);
            return result.Success ? Ok(result) :BadRequest(result);
        }
    }
}
