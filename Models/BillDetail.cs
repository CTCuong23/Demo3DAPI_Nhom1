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
        public int BillId { get; set; } 

        [Required]
        public int ProductId { get; set; } 

        [Required]
        public int Quantity { get; set; } 

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } 

        
        [ForeignKey("BillId")]
        public virtual Bill? Bill { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}