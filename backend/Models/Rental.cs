using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("rental")]
public class Rental
{
    [Key]
    public Guid RentalID { get; set; }
    
    [Required]
    public Guid StatusID { get; set; }
    
    [Required]
    public Guid ClientID { get; set; }
    
    [Required]
    public Guid UserID { get; set; }
    
    public DateTime RentalDate { get; set; } = DateTime.UtcNow;
    public DateTime RentalDateExpected { get; set; }

    // Propiedades de navegación para EF Core
    [ForeignKey("status_id")]
    public virtual Status? Status { get; set; }
    
    [ForeignKey("client_id")]
    public virtual Client? Client { get; set; }
    
    [ForeignKey("user_id")]
    public virtual User? User { get; set; }
}