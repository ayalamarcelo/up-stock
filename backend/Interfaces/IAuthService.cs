using UpStock.DTOs;

namespace UpStock.Interfaces;

public interface IAuthService
{
    Task<string?> RegisterAsync(RegisterDto registerDto);
    Task<string?> LoginAsync(LoginDto loginDto);
}