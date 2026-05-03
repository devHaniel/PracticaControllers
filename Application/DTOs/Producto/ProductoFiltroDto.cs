namespace GestionProducto.Application.DTOs.Producto;

public class ProductoFiltroDto
{
    public string? Codigo {get; set;} = string.Empty;
    public string? Nombre {get; set;} = string.Empty;
    public bool? Activo {get; set;}
}
