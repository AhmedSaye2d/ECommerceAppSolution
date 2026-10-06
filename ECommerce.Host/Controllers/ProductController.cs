using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productservice;
        public ProductController(IProductService _productservice)
        {
            this.productservice = _productservice;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await productservice.GetAllAsync();
            return data.Any() ? Ok(data) : NotFound(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQueryDto query)
        {
            var result = await productservice.GetProductsAsync(query);
            return Ok(result);
        }

        [HttpGet("Single/{id}")]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            var data = await productservice.GetByIdAsync(id);
            return data != null ? Ok(data) : NotFound(data);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateProduct Product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await productservice.AddAsync(Product);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(UpdateProduct product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await productservice.UpdateAsync(product);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await productservice.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
