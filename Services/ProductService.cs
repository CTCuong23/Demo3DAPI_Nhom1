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

        // 1. Lấy danh sách
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            // Dùng Set<Product>() cho an toàn, phòng trường hợp bạn chưa khai báo DbSet<Product>
            return await _context.Set<Product>()
                                 .Include(p => p.Category) // Kèm thông tin danh mục
                                 .ToListAsync();
        }

        // 2. Lấy chi tiết 1 sản phẩm
        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Set<Product>()
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.ID == id);
        }

        // 3. Thêm mới
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

        // 4. Cập nhật (Sửa)
        public async Task<bool> UpdateProduct(int id, UpdateProductDTO productDto)
        {
            // Tìm sản phẩm cần sửa
            var existingProduct = await _context.Set<Product>().FindAsync(id);

            // Nếu không tìm thấy thì báo lỗi (false)
            if (existingProduct == null)
            {
                return false;
            }

            // Cập nhật thông tin mới
            existingProduct.ProductName = productDto.ProductName;
            existingProduct.Price = productDto.Price;
            existingProduct.CategoryID = productDto.CategoryID;

            // Lưu vào database
            await _context.SaveChangesAsync();
            return true;
        }

        // 5. Xóa
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