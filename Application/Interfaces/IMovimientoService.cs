using GestionProducto.Application.DTOs.Movimiento;
using GestionProducto.DTOs.Movimiento;
using GestionProducto.Models;
using GestionProducto.Models.enums;

namespace GestionProducto.Application.Interfaces;

public interface IMovimientoService
{
    Task<Movimiento?> ObtenerPorId(int id);
    Task<IEnumerable<Movimiento>> ObtenerTodos();
    Task<IEnumerable<Movimiento>> Obtener(MovimientoFiltroDto filtro);
    Task<IEnumerable<MovimientoDto>> ObtenerPorIdProducto(int id);
    Task Agregar(MovimientoAgregarDto movimiento);
    Task Actualizar(MovimientoActualizarDto movimiento);
    Task Eliminar(int  id);
}
