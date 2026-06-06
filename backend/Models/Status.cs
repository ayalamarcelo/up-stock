using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("status")] 
public class Status
{
    [Key]
    public Guid statusid { get; set; } 

    [Required]
    [StringLength(150)]
    public string namestatus { get; set; } = string.Empty; 

    [StringLength(255)]
    public string? description { get; set; } 
}