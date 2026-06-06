using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context) => _context = context;

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _context.Users.Where(u => u.IsActive).ToListAsync();

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

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id && u.IsActive);

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

            var emailExiste = await _context.Users.AnyAsync(u => u.Email == user.Email && u.IsActive);
            if (emailExiste)
                return Conflict($"Ya existe un usuario con el email '{user.Email}'.");

            user.UserID = Guid.NewGuid();
            user.IsActive = true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
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

            var existe = await _context.Users.AnyAsync(u => u.UserID == id && u.IsActive);
            if (!existe)
                return NotFound($"No se encontró ningún usuario con el ID {id}.");

            var emailDuplicado = await _context.Users.AnyAsync(u => u.Email == user.Email && u.UserID != id && u.IsActive);
            if (emailDuplicado)
                return Conflict($"Ya existe otro usuario con el email '{user.Email}'.");

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id && u.IsActive);

            if (user == null)
                return NotFound($"No se encontró ningún usuario con el ID {id}.");

            user.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}