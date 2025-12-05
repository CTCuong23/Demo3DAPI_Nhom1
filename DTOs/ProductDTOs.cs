using System.ComponentModel.DataAnnotations;

namespace Demo3DAPI.DTOs
{
    // DTO dùng để tạo mới (không cần ID)
    public class CreateProductDTO
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryID { get; set; }
    }

    // DTO dùng để cập nhật (giống tạo mới nhưng tách ra để sau này dễ mở rộng)
    public class UpdateProductDTO
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryID { get; set; }
    }
}