using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Demo3DAPI.Models;

namespace Demo3DAPI.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required]
        public string Status { get; set; }

        public int PlayerAccountId { get; set; }

        [ForeignKey("PlayerAccountId")]
        public virtual PlayerAccount? PlayerAccount { get; set; }
    }
}