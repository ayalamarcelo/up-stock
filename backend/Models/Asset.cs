namespace UpStock.Models;

public class Asset
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = "Disponible";
}