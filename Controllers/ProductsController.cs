using Microsoft.AspNetCore.Mvc;
using Demo3DAPI.Interfaces;
using Demo3DAPI.DTOs;

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

        // 1. GET: api/Products (Lấy danh sách)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        // 2. GET: api/Products/5 (Lấy 1 cái theo ID)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Không tìm thấy sản phẩm có ID = {id}");
            }
            return Ok(product);
        }

        // 3. POST: api/Products (Thêm mới)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = await _productService.CreateProduct(productDto);
            // Trả về code 201 Created
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ID }, newProduct);
        }

        // 4. PUT: api/Products/5 (Sửa)
        [HttpPut("{id}")]
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

        // 5. DELETE: api/Products/5 (Xóa)
        [HttpDelete("{id}")]
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