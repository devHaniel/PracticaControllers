namespace GestionProducto.DTOs.Producto;

public class ProductoAgregarDto
{
    public string Codigo {get; set;} = string.Empty;
    public string Nombre {get; set;} = string.Empty;
    public int StockActual {get; set;}
    public int StockMinimo {get; set;}
    public decimal PrecioCompra {get; set;}
    public decimal PrecioVenta  {get; set;}
    public bool Activo {get; set;}
}
