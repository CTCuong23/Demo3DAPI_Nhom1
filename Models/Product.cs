using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Demo3DAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        // "decimal" là kiểu dữ liệu tốt nhất cho tiền tệ (PRICE)
        [Column(TypeName = "decimal(18, 2)")] // Giúp SQL lưu trữ tiền chính xác
        public decimal Price { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; }
    }
}
