using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Demo3DAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Bắt buộc phải có Token mới vào được Controller này
    public class PlayerAccountsController : ControllerBase
    {
        private readonly IPlayerAccountService _accountService;

        public PlayerAccountsController(IPlayerAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới xem được danh sách tất cả
        [SwaggerOperation(Summary = "Xem tất cả tài khoản (Admin only)")]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAccounts();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Xem một tài khoản")]
        public async Task<IActionResult> GetById(int id)
        {
            // Lấy ID và Role của người đang đăng nhập từ Token
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Nếu không phải Admin VÀ không phải chính chủ -> Chặn
            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            var account = await _accountService.GetAccountById(id);
            if (account == null)
                return NotFound(new { message = $"Account with ID {id} not found." });
            return Ok(account);
        }

        // Admin tạo tài khoản thủ công (khác với Register tự đăng ký)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Thêm tài khoản mới (Admin only)")]
        public async Task<IActionResult> Create([FromBody] CreatePlayerAccountDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newAccount = await _accountService.CreateAccount(createDto);
                return CreatedAtAction(nameof(GetById), new { id = newAccount.ID }, newAccount);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}")] // Lưu ý: Thầy dùng HttpPost cho Update (hơi lạ, thường là HttpPut, nhưng cứ theo thầy)
        [SwaggerOperation(Summary = "Sửa tài khoản")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePlayerAccountDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            var result = await _accountService.UpdateAccount(id, updateDto);
            if (!result)
                return NotFound(new { message = $"Account with ID {id} not found." });
            return Ok(new { message = "Account updated successfully." });
        }

        [HttpPost("Delete/{id}")] // Thầy dùng HttpPost cho Delete
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Xóa tài khoản (Admin only)")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _accountService.DeleteAccount(id);
                if (!result)
                    return NotFound(new { message = $"Account with ID {id} not found." });
                return Ok(new { message = "Account deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}