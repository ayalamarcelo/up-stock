using Microsoft.AspNetCore.Mvc;
using UpStock.Models;
using UpStock.Interfaces;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;
    private readonly ILogger<RentalsController> _logger;

    public RentalsController(IRentalService rentalService, ILogger<RentalsController> logger)
    {
        _rentalService = rentalService;
        _logger = logger;
    }

    // GET: api/Rentals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
    {
        try
        {
            var rentals = await _rentalService.GetAllAsync();
            return Ok(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de alquileres.");
            return StatusCode(500, new { message = "Error interno al recuperar los alquileres.", detalle = ex.Message });
        }
    }

    // GET: api/Rentals/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetRental(Guid id)
    {
        try
        {
            if (id == Guid.Empty) return BadRequest(new { message = "El ID proporcionado no es válido." });

            var rental = await _rentalService.GetByIdAsync(id);
            
            if (rental == null) 
                return NotFound(new { message = $"No se encontró el alquiler con ID {id}." });

            return Ok(rental);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener el alquiler {id}.");
            return StatusCode(500, new { message = "Error al obtener el alquiler.", detalle = ex.Message });
        }
    }

    // POST: api/Rentals
    [HttpPost]
    public async Task<ActionResult<Rental>> PostRental([FromBody] Rental rental)
    {
        try
        {
            if (rental == null) 
                return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío." });

            // agregar validaciones de negocio adicionales antes de llamar al servicio
            
            var created = await _rentalService.CreateAsync(rental);
            
            return CreatedAtAction(nameof(GetRental), new { id = created.RentalID }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar crear un nuevo alquiler.");
            return StatusCode(500, new { message = "Error interno al crear el alquiler.", detalle = ex.Message });
        }
    }
}