using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssetsController(AppDbContext context) => _context = context;

    // GET: api/Assets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        try
        {
            var assets = await _context.Assets.Where(a => !a.IsDeleted).ToListAsync();

            if (!assets.Any())
                return NotFound("No hay activos registrados en el sistema.");

            return Ok(assets);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // GET: api/Assets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetID == id && !a.IsDeleted);

            if (asset == null)
                return NotFound($"No se encontró ningún activo con el ID {id}.");

            return Ok(asset);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // POST: api/Assets
    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(Asset asset)
    {
        try
        {
            if (asset == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(asset.Name))
                return BadRequest("El nombre del activo es obligatorio.");

            if (string.IsNullOrWhiteSpace(asset.CodeID))
                return BadRequest("El código del activo es obligatorio.");

            var existe = await _context.Assets.AnyAsync(a => a.CodeID == asset.CodeID && !a.IsDeleted);
            if (existe)
                return Conflict($"Ya existe un activo con el código '{asset.CodeID}'.");

            asset.AssetID = Guid.NewGuid();
            asset.IsDeleted = false;

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsset), new { id = asset.AssetID }, asset);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // PUT: api/Assets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(Guid id, Asset asset)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            if (id != asset.AssetID)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (string.IsNullOrWhiteSpace(asset.Name))
                return BadRequest("El nombre del activo es obligatorio.");

            if (string.IsNullOrWhiteSpace(asset.CodeID))
                return BadRequest("El código del activo es obligatorio.");

            var existe = await _context.Assets.AnyAsync(a => a.AssetID == id && !a.IsDeleted);
            if (!existe)
                return NotFound($"No se encontró ningún activo con el ID {id}.");

            var codigoDuplicado = await _context.Assets.AnyAsync(a => a.CodeID == asset.CodeID && a.AssetID != id && !a.IsDeleted);
            if (codigoDuplicado)
                return Conflict($"Ya existe otro activo con el código '{asset.CodeID}'.");

            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // DELETE: api/Assets/5 (Soft Delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            var asset = await _context.Assets.FindAsync(id);

            if (asset == null || asset.IsDeleted)
                return NotFound($"No se encontró ningún activo con el ID {id}.");

            asset.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}