using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpStock.Models;

[Table("category")] 
public class Category
{
    [Key] //
    [Column("categoryid")] 
    public Guid CategoryID { get; set; } = Guid.NewGuid();

    [Required] 
    [Column("namecategory")] 
    [StringLength(150)] 
    public string NameCategory { get; set; } = string.Empty;
}