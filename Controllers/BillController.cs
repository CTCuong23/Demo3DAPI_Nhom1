using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Demo3DAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Cần cái này
using System.Security.Claims; // Cần cái này để lấy ID người dùng từ Token
using Swashbuckle.AspNetCore.Annotations;

namespace Demo3DAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 BẮT BUỘC ĐĂNG NHẬP MỚI VÀO ĐƯỢC CONTROLLER NÀY
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet("GetAll")]
        [SwaggerOperation(Summary = "Xem danh sách hóa đơn")]
        public async Task<IActionResult> GetBills()
        {
            // Lấy ID và Role từ Token người đang đăng nhập
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var bills = await _billService.GetAllBills();

            // Nếu không phải Admin thì chỉ trả về hóa đơn của chính mình
            if (role != "Admin")
            {
                bills = bills.Where(b => b.PlayerAccountId == currentUserId);
            }

            return Ok(new { data = bills });
        }

        [HttpGet("GetById/{id}")]
        [SwaggerOperation(Summary = "Xem chi tiết một hóa đơn")]
        public async Task<IActionResult> GetBill(int id)
        {
            var bill = await _billService.GetBillById(id);
            if (bill == null) return NotFound("Không tìm thấy hóa đơn");

            // Bảo mật: Kiểm tra xem hóa đơn này có phải của người đó không (trừ Admin)
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "Admin" && bill.PlayerAccountId != currentUserId)
            {
                return Forbid(); // Chặn nếu xem trộm hóa đơn người khác
            }

            return Ok(new { data = bill });
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Tạo hóa đơn mua hàng")]
        public async Task<IActionResult> PostBill([FromBody] CreateBillDto input)
        {
            try
            {
                // Lấy ID người dùng đang đăng nhập tự động (bảo mật hơn là lấy từ input)
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var newBill = new Bill
                {
                    PlayerAccountId = currentUserId, // Tự động gán ID người đang đăng nhập
                    Status = input.Status,
                    CreateDate = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    BillDetails = new List<BillDetail>()
                };

                if (input.BillDetails != null)
                {
                    foreach (var item in input.BillDetails)
                    {
                        var detail = new BillDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = 0 // Nên lấy giá từ ProductService để chuẩn xác hơn, nhưng tạm thời để 0 theo code cũ của bạn
                        };
                        newBill.BillDetails.Add(detail);
                    }
                }

                var createdBill = await _billService.CreateBill(newBill);
                return CreatedAtAction(nameof(GetBill), new { id = createdBill.Id }, createdBill);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { Error = "Lỗi tạo hóa đơn", ChiTiet = message });
            }
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được sửa trạng thái đơn hàng
        [SwaggerOperation(Summary = "Sửa hóa đơn (Admin only)")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.Id) return BadRequest("ID không khớp.");

            var result = await _billService.UpdateBill(id, bill);
            if (!result) return NotFound("Không tìm thấy Bill để cập nhật.");

            return Ok("Update Success");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được xóa hóa đơn
        [SwaggerOperation(Summary = "Xóa hóa đơn (Admin only)")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var result = await _billService.DeleteBill(id);
            if (!result) return NotFound("Không tìm thấy bill để xóa");

            return Ok("Delete Success");
        }
    }
}