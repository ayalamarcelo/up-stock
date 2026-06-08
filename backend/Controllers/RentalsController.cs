using Microsoft.AspNetCore.Mvc;
using UpStock.Models;
using UpStock.Services;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
    {
        var rentals = await _rentalService.GetAllAsync();
        return Ok(rentals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetRental(Guid id)
    {
        var rental = await _rentalService.GetByIdAsync(id);
        if (rental == null) return NotFound(new { message = "Alquiler no encontrado." });
        return Ok(rental);
    }

    [HttpPost]
    public async Task<ActionResult<Rental>> PostRental(Rental rental)
    {
        var created = await _rentalService.CreateAsync(rental);
        return CreatedAtAction(nameof(GetRental), new { id = created.RentalID }, created);
    }
}