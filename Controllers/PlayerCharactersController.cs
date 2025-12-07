using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Demo3DAPI.DTOs;
using Demo3DAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations; // Thư viện để hiện mô tả đẹp

namespace Demo3DAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Bắt buộc đăng nhập
    public class PlayerCharactersController : ControllerBase
    {
        private readonly IPlayerCharacterService _characterService;

        public PlayerCharactersController(IPlayerCharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Xem tất cả nhân vật (Admin only)", Description = "Lấy danh sách toàn bộ nhân vật trên hệ thống.")]
        [SwaggerResponse(200, "Lấy dữ liệu thành công")]
        [SwaggerResponse(401, "Chưa đăng nhập")]
        [SwaggerResponse(403, "Không có quyền truy cập (Phải là Admin)")]
        public async Task<IActionResult> GetAll()
        {
            var characters = await _characterService.GetAllCharacters();
            return Ok(characters);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Xem chi tiết nhân vật", Description = "Xem thông tin nhân vật theo ID (Chỉ xem được nhân vật của mình, Admin xem được tất cả).")]
        [SwaggerResponse(200, "Tìm thấy nhân vật")]
        [SwaggerResponse(404, "Không tìm thấy nhân vật")]
        [SwaggerResponse(403, "Không được phép xem nhân vật của người khác")]
        public async Task<IActionResult> GetById(int id)
        {
            var character = await _characterService.GetCharacterById(id);
            if (character == null)
                return NotFound(new { message = $"Character with ID {id} not found." });

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Kiểm tra: Nhân vật này có thuộc về người đang đăng nhập không?
            if (currentUserRole != "Admin" && character.PlayerAccountID != currentUserId)
            {
                return Forbid();
            }

            return Ok(character);
        }

        [HttpGet("Account/{accountId}")]
        [SwaggerOperation(Summary = "Xem danh sách nhân vật theo tài khoản", Description = "Lấy tất cả nhân vật thuộc về một Account ID cụ thể.")]
        public async Task<IActionResult> GetByAccountId(int accountId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && currentUserId != accountId)
            {
                return Forbid();
            }

            var characters = await _characterService.GetCharactersByAccountId(accountId);
            return Ok(characters);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Tạo nhân vật mới", Description = "Tạo nhân vật game mới (Level mặc định là 1).")]
        [SwaggerResponse(201, "Tạo thành công")]
        [SwaggerResponse(400, "Dữ liệu không hợp lệ")]
        public async Task<IActionResult> Create([FromBody] CreatePlayerCharacterDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Không cho phép tạo nhân vật giùm người khác (trừ Admin)
            if (currentUserRole != "Admin" && createDto.PlayerAccountID != currentUserId)
            {
                return Forbid();
            }

            try
            {
                var newCharacter = await _characterService.CreateCharacter(createDto);
                if (newCharacter == null) return BadRequest(new { message = "PlayerAccountID does not exist." });

                return CreatedAtAction(nameof(GetById), new { id = newCharacter.ID }, newCharacter);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}")] // Lưu ý: Thầy dùng HttpPost cho Update
        [SwaggerOperation(Summary = "Cập nhật nhân vật", Description = "Cập nhật tên hoặc level nhân vật.")]
        [SwaggerResponse(200, "Cập nhật thành công")]
        [SwaggerResponse(404, "Không tìm thấy nhân vật")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePlayerCharacterDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var character = await _characterService.GetCharacterById(id);
            if (character == null) return NotFound(new { message = $"Character with ID {id} not found." });

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && character.PlayerAccountID != currentUserId)
            {
                return Forbid();
            }

            try
            {
                var result = await _characterService.UpdateCharacter(id, updateDto);
                if (!result) return NotFound(new { message = $"Character with ID {id} not found." });
                return Ok(new { message = "Character updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Delete/{id}")] // Lưu ý: Thầy dùng HttpPost cho Delete
        [SwaggerOperation(Summary = "Xóa nhân vật", Description = "Xóa hoàn toàn nhân vật khỏi hệ thống.")]
        [SwaggerResponse(200, "Xóa thành công")]
        [SwaggerResponse(404, "Không tìm thấy nhân vật")]
        public async Task<IActionResult> Delete(int id)
        {
            var character = await _characterService.GetCharacterById(id);
            if (character == null) return NotFound(new { message = $"Character with ID {id} not found." });

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && character.PlayerAccountID != currentUserId)
            {
                return Forbid();
            }

            var result = await _characterService.DeleteCharacter(id);
            if (!result) return NotFound(new { message = $"Character with ID {id} not found." });
            return Ok(new { message = "Character deleted successfully." });
        }
    }
}
// Test thử git trên đt