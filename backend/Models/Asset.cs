using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("assets")] 
public class Asset
{
    [Key]
    public Guid assetid { get; set; }

    [Required]
    public Guid categoryid { get; set; }

    [Required]
    public Guid statusid { get; set; }

    [Required]
    [StringLength(150)]
    public string name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string codeid { get; set; } = string.Empty;

    public bool isdeleted { get; set; } = false;
}