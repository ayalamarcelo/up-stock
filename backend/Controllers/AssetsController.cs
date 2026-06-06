using UpStock.Models;
using UpStock.Services;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        var assets = await _assetService.GetAllAsync();
        return Ok(assets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(Guid id)
    {
        var asset = await _assetService.GetByIdAsync(id);
        if (asset == null) return NotFound(new { message = "Activo no encontrado" });
        return Ok(asset);
    }

    [HttpPost]
    public async Task<ActionResult<Asset>> PostAsset(Asset asset)
    {
        var createdAsset = await _assetService.CreateAsync(asset);
        return CreatedAtAction(nameof(GetAsset), new { id = createdAsset.assetid }, createdAsset);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsset(Guid id, Asset asset)
    {
        var result = await _assetService.UpdateAsync(id, asset);
        if (!result) return BadRequest(new { message = "Error al actualizar el activo" });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(Guid id)
    {
        var result = await _assetService.DeleteAsync(id);
        if (!result) return NotFound(new { message = "Activo no encontrado" });
        return NoContent();
    }
}