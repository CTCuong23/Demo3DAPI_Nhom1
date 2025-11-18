using System.ComponentModel.DataAnnotations;

namespace Demo3DAPI.DTOs
{
    // Dùng khi tạo mới
    public class CreateProductDto
    {
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? Dis { get; set; } 
    }

    
    public class UpdateProductDto
    {
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? Dis { get; set; } 
    }
}