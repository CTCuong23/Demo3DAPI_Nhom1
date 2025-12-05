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

        
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        
        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

   
        public async Task<Product> CreateProduct(CreateProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                Dis = productDto.Dis 
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product; 
        }

        
        public async Task<bool> UpdateProduct(int id, UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

           
            product.ProductName = productDto.ProductName;
            product.Price = productDto.Price;
            product.Dis = productDto.Dis; 

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

      
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}