using UpStock.Models;
using UpStock.Services;
using Microsoft.AspNetCore.Mvc;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    // Inyectamos el servicio en lugar del DbContext directamente
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    // GET: api/category/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound(new { message = "Categoría no encontrada" });

        return Ok(category);
    }

    // POST: api/category
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        var createdCategory = await _categoryService.CreateAsync(category);
        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryID }, createdCategory);
    }

    // PUT: api/category/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(Guid id, Category category)
    {
        var result = await _categoryService.UpdateAsync(id, category);
        if (!result) return BadRequest(new { message = "Error al actualizar la categoría" });

        return NoContent();
    }

    // DELETE: api/category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result) return NotFound(new { message = "Categoría no encontrada para eliminar" });

        return NoContent();
    }
}