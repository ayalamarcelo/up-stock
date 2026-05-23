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
        return await _context.Assets.Where(a => !a.IsDeleted).ToListAsync();
    }

    // GET: api/Assets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(Guid id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetID == id && !a.IsDeleted);
        if (asset == null) return NotFound();
        return asset;
    }

    // POST: api/Assets
    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(Asset asset)
    {
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAsset), new { id = asset.AssetID }, asset);
    }

    // PUT: api/Assets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(Guid id, Asset asset)
    {
        if (id != asset.AssetID) return BadRequest();
        
        _context.Entry(asset).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Assets/5 (Soft Delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(Guid id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null || asset.IsDeleted) return NotFound();

        asset.IsDeleted = true; // Solo marcamos como borrado
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}