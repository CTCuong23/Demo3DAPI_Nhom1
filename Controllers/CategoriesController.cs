using Microsoft.AspNetCore.Mvc;
using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Demo3DAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
namespace Demo3DAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        [SwaggerOperation(Summary = "Xem tất cả danh mục")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("GetById/{id}")] 
        [SwaggerOperation(Summary = "Xem một danh mục")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Thêm danh mục mới")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO createDto)
        {
            try
            {
                var newCategory = await _categoryService.CreateCategory(createDto);
                return CreatedAtAction(nameof(GetById), new { id = newCategory.ID }, newCategory);
            }
            catch (InvalidOperationException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Update/{id}")]
        [SwaggerOperation(Summary = "Sửa danh mục")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO updateDto)
        {
            var result = await _categoryService.UpdateCategory(id, updateDto);
            if (!result)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }
            return Ok(new { message = "Category updated successfully." });
        }

        [HttpPost("Delete/{id}")]
        [SwaggerOperation(Summary = "Xóa danh mục")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
