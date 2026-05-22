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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        return await _context.Assets.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(Asset asset)
    {
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAssets), new { id = asset.Id }, asset);
    }
}