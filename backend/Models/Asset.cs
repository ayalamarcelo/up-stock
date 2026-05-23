using System.ComponentModel.DataAnnotations;

namespace UpStock.Models;

public class Asset
{
    [Key]
    public Guid AssetID { get; set; } // Guid es el equivalente a uuid de postgre en c#
    
    public Guid CategoryID { get; set; }
    public Guid StatusID { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string CodeID { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}