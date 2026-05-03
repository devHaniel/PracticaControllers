using GestionProducto.Models.enums;

namespace GestionProducto.Application.DTOs.Movimiento;

public class MovimientoFiltroDto
{
    public int? ProductoId { get; set; }
    public TipoMovimiento? Tipo { get; set; }

    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public string? Motivo { get; set; }
}
