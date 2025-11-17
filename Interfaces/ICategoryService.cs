using Demo3DAPI.DTOs;
using Demo3DAPI.Models;
namespace Demo3DAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();

        Task<Category?> GetCategoryById(int id);

        Task<Category> CreateCategory(CreateCategoryDTO categoryDto);

        Task<bool> UpdateCategory(int id, UpdateCategoryDTO categoryDto);

        Task<bool> DeleteCategory(int id);
    }
}
