using System.ComponentModel.DataAnnotations;

namespace UpStock.Models;

public class User
{
    [Key]
    public Guid UserID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Rol { get; set; } = "Employee";
    
    public bool IsActive { get; set; } = true;
}