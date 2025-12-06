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
    [Authorize]
    public class PlayerCharactersController : ControllerBase
    {
        private readonly IPlayerCharacterService _characterService;

        public PlayerCharactersController(IPlayerCharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var characters = await _characterService.GetAllCharacters();
            return Ok(characters);
        }

        [HttpGet("{id}")]
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

        [HttpPost("{id}")]
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

        [HttpPost("Delete/{id}")]
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