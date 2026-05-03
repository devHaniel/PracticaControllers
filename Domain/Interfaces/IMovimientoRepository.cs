using GestionProducto.Models;
using GestionProducto.Models.enums;

namespace GestionProducto.Domain.Interfaces;

public interface IMovimientoRepository
{
    Task<Movimiento?> ObtenerPorId(int id);
    Task<IEnumerable<Movimiento>> ObtenerTodos();
    Task<IEnumerable<Movimiento>> ObtenerPorTIpo(TipoMovimiento tipoMovimiento);
    Task<IEnumerable<Movimiento>> ObtenerPorMotivo(string motivo);
    Task<IEnumerable<Movimiento>> ObtenerPorFecha(DateTime fecha);
    Task<IEnumerable<Movimiento>> ObtenerPorIdProducto(int id);
    Task Agregar(Movimiento movimiento);
    Task Actualizar(Movimiento movimiento);
    Task Eliminar(Movimiento movimiento);

}
