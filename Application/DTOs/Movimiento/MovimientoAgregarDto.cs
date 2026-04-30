using GestionProducto.Models.enums;

namespace GestionProducto.DTOs.Movimiento;

public class MovimientoAgregarDto
{
    public int ProductoId {get; set;}
    public TipoMovimiento Tipo {get; set;}
    public string Motivo {get; set;} = string.Empty;
    public int Cantidad {get; set;}
}
