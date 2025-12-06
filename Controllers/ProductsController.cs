using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Cần cái này để bảo mật
using Demo3DAPI.Interfaces;
using Demo3DAPI.DTOs;
using Demo3DAPI.Models;
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

        // Ai cũng xem được, không cần đăng nhập
        [HttpGet]
        [SwaggerOperation(Summary = "Lấy danh sách sản phẩm", Description = "Trả về danh sách tất cả sản phẩm hiện có")]
        [SwaggerResponse(200, "Lấy dữ liệu thành công", typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        // Ai cũng xem được
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Lấy chi tiết sản phẩm", Description = "Tìm và trả về thông tin sản phẩm theo ID")]
        [SwaggerResponse(200, "Tìm thấy sản phẩm", typeof(Product))]
        [SwaggerResponse(404, "Không tìm thấy sản phẩm")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Không tìm thấy sản phẩm có ID = {id}");
            }
            return Ok(product);
        }

        // 🔒 CHỈ ADMIN ĐƯỢC THÊM
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Thêm sản phẩm mới (Admin only)")]
        [SwaggerResponse(201, "Tạo thành công", typeof(Product))]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ")]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = await _productService.CreateProduct(productDto);

            return CreatedAtAction(nameof(GetById), new { id = newProduct.ID }, newProduct);
        }

        // 🔒 CHỈ ADMIN ĐƯỢC SỬA
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Cập nhật sản phẩm (Admin only)")]
        [SwaggerResponse(200, "Cập nhật thành công")]
        [SwaggerResponse(404, "Không tìm thấy sản phẩm để sửa")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.UpdateProduct(id, productDto);

            if (!result)
            {
                return NotFound($"Không tìm thấy sản phẩm để sửa (ID = {id})");
            }

            return Ok("Cập nhật thành công!");
        }

        // 🔒 CHỈ ADMIN ĐƯỢC XÓA
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Xóa sản phẩm (Admin only)")]
        [SwaggerResponse(200, "Xóa thành công")]
        [SwaggerResponse(404, "Không tìm thấy sản phẩm để xóa")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProduct(id);

            if (!result)
            {
                return NotFound($"Không tìm thấy sản phẩm để xóa (ID = {id})");
            }

            return Ok("Xóa thành công!");
        }
    }
}