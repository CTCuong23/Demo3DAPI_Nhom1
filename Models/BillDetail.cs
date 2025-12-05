using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo3DAPI.Models
{
    public class BillDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int BillId { get; set; } // Khóa ngoại trỏ về Bill

        [Required]
        public int ProductId { get; set; } // Khóa ngoại trỏ về Product

        [Required]
        public int Quantity { get; set; } // Số lượng mua (ví dụ mua 5 bình máu)

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } // Giá tại thời điểm mua (để lỡ sau này Product tăng giá thì hóa đơn cũ không bị sai)

        // Navigation Properties (Cầu nối)
        [ForeignKey("BillId")]
        public virtual Bill? Bill { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}