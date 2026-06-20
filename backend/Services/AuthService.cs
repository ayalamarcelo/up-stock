using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UpStock.Data;
using UpStock.DTOs;
using UpStock.Interfaces;
using UpStock.Models;

namespace UpStock.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string?> RegisterAsync(RegisterDto registerDto)
    {
        // 1. Validar si el email ya está registrado
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            return null;
        }

        // 2. Encriptar la contraseña usando BCrypt
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, salt);

        // 3. Crear la entidad User
        var newUser = new User
        {
            UserID = Guid.NewGuid(),
            Email = registerDto.Email,
            PasswordHash = hashedPassword,
            Rol = "Employee",
            IsActive = true
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // 4. Generar y devolver el token JWT directamente tras registrarse
        return GenerateJwtToken(newUser);
    }

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        // 1. Buscar al usuario por Email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null || !user.IsActive)
        {
            return null;
        }

        // 2. Verificar si la contraseña coincide con el Hash guardado
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null;
        }

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "SuperSecretKeyForUpStockJWTAuth2026";
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "UpStockAPI";
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "UpStockClient";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Rol)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}