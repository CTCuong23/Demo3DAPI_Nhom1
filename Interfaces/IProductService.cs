using Demo3DAPI.DTOs;
using Demo3DAPI.Models;

namespace Demo3DAPI.Interfaces
{
    public interface IProductService
    {
        
        Task<IEnumerable<Product>> GetAllProducts();

        
        Task<Product?> GetProductById(int id);

        
        Task<Product> CreateProduct(CreateProductDTO productDto);

        
        Task<bool> UpdateProduct(int id, UpdateProductDTO productDto);

        
        Task<bool> DeleteProduct(int id);
    }
}