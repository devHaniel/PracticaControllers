namespace GestionProducto.DTOs.Producto;

public class ProductoActualizarDto
{
    public int Id {get; set;}
    public string Nombre {get; set;} = string.Empty;
    public int StockMinimo {get; set;}
    public decimal PrecioVenta  {get; set;}
    public bool Activo {get; set;}
}
