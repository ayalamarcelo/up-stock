using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("assets")] 
public class Asset
{
    [Key]
    public Guid AssetId { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public Guid StatusId { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CodeId { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;
}