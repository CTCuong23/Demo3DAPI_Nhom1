using Demo3DAPI.Data;
using Demo3DAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Demo3DAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        [SwaggerOperation(Summary = "Xem tất cả hóa đơn", Description = "Lấy danh sách tất cả các hóa đơn")]
        [SwaggerResponse(200, "Thành công", typeof(IEnumerable<PlayerAccount>))]
        public async Task<IActionResult> GetBills()
        {
            try
            {
                var data = await _context.Bills
                                         .Include(b => b.PlayerAccount) 
                                         .ToListAsync();
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public async Task<IActionResult> GetAll()
        //{
        //    var accounts = await _context.GetAllBill();
        //    return Ok(accounts);
        //}

        [HttpGet("GetById/{id}")]
        [SwaggerOperation(Summary = "Xem một hóa đơn", Description = "Lấy thông tin chi tiết hóa đơn theo ID")]
        [SwaggerResponse(200, "Thành công", typeof(Bill))]
        [SwaggerResponse(404, "Không tìm thấy hóa đơn")]
        public async Task<IActionResult> GetBill(int id)
        {
            try
            {
                var data = await _context.Bills
                                         .Include(b => b.PlayerAccount)
                                         .FirstOrDefaultAsync(b => b.Id == id);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Thêm hóa đơn mới", Description = "Tạo hóa đơn mới")]
        [SwaggerResponse(201, "Tạo thành công", typeof(PlayerAccount))]
        [SwaggerResponse(400, "Dữ liệu không hợp lệ")]
        public async Task<IActionResult> PostBill(Bill bill)
        {
            try
            {
                bill.CreateDate = DateTime.Now;

                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();
                return Ok(new { data = bill });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Update/{id}")]
        [SwaggerOperation(Summary = "Sửa hóa đơn", Description = "Cập nhật thông tin hóa đơn theo ID")]
        [SwaggerResponse(200, "Cập nhật thành công")]
        [SwaggerResponse(404, "Không tìm thấy hóa đơn")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.Id)
            {
                return BadRequest("ID không khớp.");
            }

            try
            {
                var billToUpdate = await _context.Bills.FindAsync(id);
                if (billToUpdate == null)
                {
                    return NotFound("Không tìm thấy Bill để cập nhật.");
                }

                billToUpdate.PaymentDate = bill.PaymentDate;
                billToUpdate.Status = bill.Status;

                await _context.SaveChangesAsync();
                return Ok("Update Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Delete/{id}")]
        [SwaggerOperation(Summary = "Xóa hóa đơn", Description = "Xóa hóa đơn theo ID (tự động xóa tất cả dữ liệu liên quan)")]
        [SwaggerResponse(200, "Xóa thành công")]
        [SwaggerResponse(404, "Không tìm thấy hóa đơn")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            try
            {
                var bill = await _context.Bills.FindAsync(id);
                if (bill == null)
                {
                    return NotFound("Không tìm thấy bill để xóa");
                }

                _context.Bills.Remove(bill);
                await _context.SaveChangesAsync();
                return Ok("Delete Success"); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}