using GestionProducto.Models.enums;

namespace GestionProducto.DTOs.Movimiento;

public class MovimientoDto
{
    public int Id {get; set;}
    public int ProductoId {get; set;}
    public TipoMovimiento Tipo {get; set;}
    public string Motivo {get; set;} = string.Empty;
    public int Cantidad {get; set;}
    public DateTime Fecha {get; set;}
}
