using System.ComponentModel.DataAnnotations;

namespace Demo3DAPI.DTOs
{
    // 1. Cái này định nghĩa 1 dòng trong hóa đơn (Mua cái gì, bao nhiêu cái)
    public class CreateBillDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // 2. Cái này định nghĩa cả hóa đơn (Của ai, trạng thái gì, danh sách hàng)
    public class CreateBillDto
    {
        public int PlayerAccountId { get; set; } // ID người mua
        public string Status { get; set; } = "Nợ"; // Mặc định là Nợ

        // Danh sách các món hàng mua
        public List<CreateBillDetailDto> BillDetails { get; set; } = new();
    }
}