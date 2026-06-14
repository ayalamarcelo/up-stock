namespace UpStock.Models;

public class Client
{
    public Guid ClientID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DniCuit { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}