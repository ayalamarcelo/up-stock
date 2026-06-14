using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("rental_items")]
public class RentalItem
{
    [Key]
    public Guid RentalItemID { get; set; }

    [Required]
    public Guid RentalID { get; set; }

    [Required]
    public Guid AssetID { get; set; }

    // Propiedades de navegación
    [ForeignKey("rental_id")]
    public virtual Rental? Rental { get; set; }

    [ForeignKey("asset_id")]
    public virtual Asset? Asset { get; set; }
}