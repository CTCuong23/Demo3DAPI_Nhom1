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

       
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Price { get; set; }

     
        [Column(TypeName = "nvarchar(max)")]
        public string? Dis { get; set; }

    }
}