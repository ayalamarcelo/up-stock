using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("status")] 
public class Status
{
    [Key]
    public Guid StatusId { get; set; } 

    [Required]
    [StringLength(150)]
    public string NameStatus { get; set; } = string.Empty; 

    [StringLength(255)]
    public string? Description { get; set; } 
}