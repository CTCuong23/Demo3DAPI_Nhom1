using System.ComponentModel.DataAnnotations;

namespace Demo3DAPI.DTOs
{
   
    public class CreateProductDTO
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryID { get; set; }
    }

    
    public class UpdateProductDTO
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryID { get; set; }
    }
}