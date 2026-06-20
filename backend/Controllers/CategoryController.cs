using UpStock.Models;
using UpStock.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            var categories = await _categoryService.GetAllAsync(
                page,
                pageSize);

            if (!categories.Any())
                return NotFound(new { message = "No hay categorías registradas en el sistema." });

            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // GET: api/Category/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound(new { message = $"No se encontró ninguna categoría con el ID {id}." });

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // POST: api/Category
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        try
        {
            if (category == null)
                return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío." });

            if (string.IsNullOrWhiteSpace(category.NameCategory))
                return BadRequest(new { message = "El nombre de la categoría es obligatorio." });

            var createdCategory = await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryID }, createdCategory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // PUT: api/Category/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(Guid id, Category category)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            if (id != category.CategoryID)
                return BadRequest(new { message = "El ID de la URL no coincide con el ID del cuerpo de la solicitud." });

            if (string.IsNullOrWhiteSpace(category.NameCategory))
                return BadRequest(new { message = "El nombre de la categoría es obligatorio." });

            var existe = await _categoryService.GetByIdAsync(id);
            if (existe == null)
                return NotFound(new { message = $"No se encontró ninguna categoría con el ID {id}." });

            var result = await _categoryService.UpdateAsync(id, category);

            if (!result)
                return BadRequest(new { message = "Error al actualizar la categoría." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // DELETE: api/Category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            var existe = await _categoryService.GetByIdAsync(id);
            if (existe == null)
                return NotFound(new { message = $"No se encontró ninguna categoría con el ID {id}." });

            var result = await _categoryService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "Categoría no encontrada para eliminar." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }
}