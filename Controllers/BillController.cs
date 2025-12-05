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
        public async Task<IActionResult> PostBill(Bill bill)
        {
            try
            {
                var newBill = await _billService.CreateBill(bill);
                return CreatedAtAction(nameof(GetBill), new { id = newBill.Id }, newBill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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