using Demo3DAPI.DTOs;
using Demo3DAPI.Models;

namespace Demo3DAPI.Interfaces
{
    public interface IProductService
    {
      
        Task<List<Product>> GetAllProducts();

       
        Task<Product?> GetProductById(int id);

      
        Task<Product> CreateProduct(CreateProductDto productDto);

     
        Task<bool> UpdateProduct(int id, UpdateProductDto productDto);

      
        Task<bool> DeleteProduct(int id);
    }
}