using Demo3DAPI.DTOs;
using Demo3DAPI.Models;

namespace Demo3DAPI.Interfaces
{
    public interface IProductService
    {
        // Lấy danh sách
        Task<IEnumerable<Product>> GetAllProducts();

        // Lấy 1 cái theo ID
        Task<Product?> GetProductById(int id);

        // Thêm
        Task<Product> CreateProduct(CreateProductDTO productDto);

        // Sửa (Trả về true/false để biết thành công hay thất bại)
        Task<bool> UpdateProduct(int id, UpdateProductDTO productDto);

        // Xóa
        Task<bool> DeleteProduct(int id);
    }
}