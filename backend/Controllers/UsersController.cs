using Microsoft.AspNetCore.Mvc;
using UpStock.Models;
using UpStock.Interfaces;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetAllAsync();

            if (!users.Any())
                return NotFound("No hay usuarios registrados en el sistema.");

            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"No se encontró ningún usuario con el ID {id}.");

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try
        {
            if (user == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(user.Name))
                return BadRequest("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("El email del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("La contraseña del usuario es obligatoria.");

            if (string.IsNullOrWhiteSpace(user.Rol))
                return BadRequest("El rol del usuario es obligatorio.");

            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.UserID }, created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, User user)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            if (id != user.UserID)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (string.IsNullOrWhiteSpace(user.Name))
                return BadRequest("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("El email del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(user.Rol))
                return BadRequest("El rol del usuario es obligatorio.");

            var updated = await _userService.UpdateAsync(id, user);
            if (!updated)
                return NotFound($"No se encontró ningún usuario con el ID {id}.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // DELETE: api/Users/5 (Soft Delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"No se encontró ningún usuario con el ID {id}.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}