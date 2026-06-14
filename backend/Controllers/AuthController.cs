using Microsoft.AspNetCore.Mvc;
using UpStock.DTOs;
using UpStock.Interfaces;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")] // Esto mapea a: api/auth
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")] // POST: api/auth/register
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.RegisterAsync(registerDto);
        
        if (token == null)
        {
            return BadRequest(new { message = "El correo electrónico ya está registrado." });
        }

        return Ok(new { token });
    }

    [HttpPost("login")] // POST: api/auth/login
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.LoginAsync(loginDto);

        if (token == null)
        {
            return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo." });
        }

        return Ok(new { token });
    }
}