using Demo3DAPI.Data;
using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Demo3DAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo3DAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
           
            return await _context.Set<Product>()
                                 .Include(p => p.Category) 
                                 .ToListAsync();
        }

        
        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Set<Product>()
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.ID == id);
        }

        
        public async Task<Product> CreateProduct(CreateProductDTO productDto)
        {
            var newProduct = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                CategoryID = productDto.CategoryID
            };

            _context.Set<Product>().Add(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }

       
        public async Task<bool> UpdateProduct(int id, UpdateProductDTO productDto)
        {
            
            var existingProduct = await _context.Set<Product>().FindAsync(id);

            
            if (existingProduct == null)
            {
                return false;
            }

            
            existingProduct.ProductName = productDto.ProductName;
            existingProduct.Price = productDto.Price;
            existingProduct.CategoryID = productDto.CategoryID;

           
            await _context.SaveChangesAsync();
            return true;
        }

       
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Set<Product>().FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Set<Product>().Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}