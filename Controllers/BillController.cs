using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Demo3DAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Demo3DAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet("GetAll")]
        [SwaggerOperation(Summary = "Xem tất cả hóa đơn")]
        public async Task<IActionResult> GetBills()
        {
            var bills = await _billService.GetAllBills();
            return Ok(new { data = bills });
        }

        [HttpGet("GetById/{id}")]
        [SwaggerOperation(Summary = "Xem một hóa đơn")]
        public async Task<IActionResult> GetBill(int id)
        {
            var bill = await _billService.GetBillById(id);
            if (bill == null) return NotFound("Không tìm thấy hóa đơn");
            return Ok(new { data = bill });
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Thêm hóa đơn mới")]
        // Đổi tham số: Nhận vào CreateBillDto thay vì Bill
        public async Task<IActionResult> PostBill([FromBody] CreateBillDto input)
        {
            try
            {
                // --- BƯỚC CHUYỂN DỮ LIỆU (MAPPING) ---

                // 1. Tạo vỏ Hóa Đơn
                var newBill = new Bill
                {
                    PlayerAccountId = input.PlayerAccountId,
                    Status = input.Status,
                    CreateDate = DateTime.Now,  // Tự động lấy giờ hiện tại
                    PaymentDate = DateTime.Now,
                    BillDetails = new List<BillDetail>()
                };

                // 2. Tạo ruột (Chi tiết hóa đơn)
                if (input.BillDetails != null)
                {
                    foreach (var item in input.BillDetails)
                    {
                        var detail = new BillDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            // Mẹo: Tạm thời set giá = 0. 
                            // Đúng chuẩn là phải gọi ProductService để lấy giá từ DB
                            UnitPrice = 0
                        };
                        newBill.BillDetails.Add(detail);
                    }
                }

                // --- GỌI SERVICE LƯU XUỐNG DB ---
                var createdBill = await _billService.CreateBill(newBill);

                return CreatedAtAction(nameof(GetBill), new { id = createdBill.Id }, createdBill);
            }
            catch (Exception ex)
            {
                // In lỗi chi tiết ra để dễ sửa
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { Error = "Lỗi tạo hóa đơn", ChiTiet = message });
            }
        }

        [HttpPut("Update/{id}")]
        [SwaggerOperation(Summary = "Sửa hóa đơn")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.Id) return BadRequest("ID không khớp.");

            var result = await _billService.UpdateBill(id, bill);
            if (!result) return NotFound("Không tìm thấy Bill để cập nhật.");

            return Ok("Update Success");
        }

        [HttpDelete("Delete/{id}")]
        [SwaggerOperation(Summary = "Xóa hóa đơn")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var result = await _billService.DeleteBill(id);
            if (!result) return NotFound("Không tìm thấy bill để xóa");

            return Ok("Delete Success");
        }
    }
}