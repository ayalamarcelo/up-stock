using UpStock.Models;
using UpStock.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetController(IAssetService service)
    {
        _assetService = service;
    }

    // GET: api/Asset
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets(
          int page = 1,
          int pageSize = 10
    )
    {
        try
        {
            var assets = await _assetService.GetAllAsync(
                page,
                pageSize
            );

            if (!assets.Any())
                return NotFound(new { message = "No hay activos registrados en el sistema." });

            return Ok(assets);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // GET: api/Asset/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            var asset = await _assetService.GetByIdAsync(id);

            if (asset == null)
                return NotFound(new { message = $"No se encontró ningún activo con el ID {id}." });

            return Ok(asset);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // POST: api/Asset
    [HttpPost]
    public async Task<ActionResult<Asset>> PostAsset(Asset asset)
    {
        try
        {
            if (asset == null)
                return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío." });

            if (string.IsNullOrWhiteSpace(asset.Name))
                return BadRequest(new { message = "El nombre del activo es obligatorio." });

            if (string.IsNullOrWhiteSpace(asset.CodeId))
                return BadRequest(new { message = "El código del activo es obligatorio." });

            if (asset.CategoryId == Guid.Empty)
                return BadRequest(new { message = "La categoría del activo es obligatoria." });

            if (asset.StatusId == Guid.Empty)
                return BadRequest(new { message = "El estado del activo es obligatorio." });

            var createdAsset = await _assetService.CreateAsync(asset);
            return CreatedAtAction(nameof(GetAsset), new { id = createdAsset.AssetId }, createdAsset);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // PUT: api/Asset/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsset(Guid id, Asset asset)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            if (id != asset.AssetId)
                return BadRequest(new { message = "El ID de la URL no coincide con el ID del cuerpo de la solicitud." });

            if (string.IsNullOrWhiteSpace(asset.Name))
                return BadRequest(new { message = "El nombre del activo es obligatorio." });

            if (string.IsNullOrWhiteSpace(asset.CodeId))
                return BadRequest(new { message = "El código del activo es obligatorio." });

            var result = await _assetService.UpdateAsync(id, asset);

            if (!result)
                return NotFound(new { message = $"No se encontró ningún activo con el ID {id}." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }
    // DELETE: api/Asset/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            var existeAsset = await _assetService.GetByIdAsync(id);
            if (existeAsset == null)
                return NotFound(new { message = $"No se encontró ningún activo con el ID {id}." });

            var result = await _assetService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "Activo no encontrado." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }
}