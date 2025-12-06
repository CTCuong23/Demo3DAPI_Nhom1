using System.ComponentModel.DataAnnotations;

namespace Demo3DAPI.DTOs
{
    
    public class CreateBillDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    
    public class CreateBillDto
    {
        public int PlayerAccountId { get; set; } 
        public string Status { get; set; } = "Nợ"; 

        public List<CreateBillDetailDto> BillDetails { get; set; } = new();
    }
}