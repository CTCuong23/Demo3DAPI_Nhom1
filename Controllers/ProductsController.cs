using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Demo3DAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Demo3DAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        
        [HttpGet("GetAll")] 
        [SwaggerOperation(Summary = "Lấy danh sách sản phẩm", Description = "Lấy toàn bộ danh sách sản phẩm có trong database")]
        [SwaggerResponse(200, "Lấy danh sách thành công", typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

       
        [HttpGet("GetById/{id}")] 
        [SwaggerOperation(Summary = "Xem chi tiết sản phẩm", Description = "Lấy thông tin chi tiết của 1 sản phẩm bằng ID")]
        [SwaggerResponse(200, "Tìm thấy sản phẩm", typeof(Product))]
        [SwaggerResponse(404, "Không tìm thấy sản phẩm")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Không tìm thấy sản phẩm với ID = {id}");
            }
            return Ok(product);
        }

       
        [HttpPost("Create")] 
        [SwaggerOperation(Summary = "Tạo sản phẩm mới", Description = "Tạo 1 sản phẩm mới (ID tự tăng)")]
        [SwaggerResponse(201, "Tạo thành công", typeof(Product))]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = await _productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.ID }, newProduct);
        }

        
        [HttpPost("Update/{id}")] 
        [SwaggerOperation(Summary = "Cập nhật sản phẩm", Description = "Cập nhật thông tin 1 sản phẩm bằng ID")]
        [SwaggerResponse(204, "Cập nhật thành công")]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ")]
        [SwaggerResponse(404, "Không tìm thấy sản phẩm")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.UpdateProduct(id, productDto);
            if (!result)
            {
                return NotFound($"Không tìm thấy sản phẩm với ID = {id}");
            }

            return NoContent();
        }

      
        [HttpPost("Delete/{id}")] 
        [SwaggerOperation(Summary = "Xóa sản phẩm", Description = "Xóa 1 sản phẩm bằng ID")]
        [SwaggerResponse(204, "Xóa thành công")]
        [SwaggerResponse(404, "Không tìm thấy sản phẩm")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result)
            {
                return NotFound($"Không tìm thấy sản phẩm với ID = {id}");
            }

            return NoContent();
        }
    }
}