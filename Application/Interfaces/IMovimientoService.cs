using GestionProducto.Application.DTOs.Movimiento;
using GestionProducto.DTOs.Movimiento;

namespace GestionProducto.Application.Interfaces;

public interface IMovimientoService
{
    Task<MovimientoDto?> ObtenerPorId(int id);
    Task<IEnumerable<MovimientoDto>> ObtenerTodos();
    Task<IEnumerable<MovimientoDto>> Obtener(MovimientoFiltroDto filtro);
    Task<IEnumerable<MovimientoDto>> ObtenerPorIdProducto(int id);
    Task<MovimientoDto> Agregar(MovimientoAgregarDto movimiento);
    Task<MovimientoDto> Actualizar(MovimientoActualizarDto movimiento);
    Task Eliminar(int id);
}
