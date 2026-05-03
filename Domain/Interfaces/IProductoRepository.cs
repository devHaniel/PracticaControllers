using GestionProducto.Models;

namespace GestionProducto.Domain.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> ObtenerPorId(int id);
    Task<IEnumerable<Producto>> ObtenerTodos();
    Task<bool> ExistePorCodigo(string codigo);
    Task Agregar(Producto producto);
    Task Actualizar(Producto producto);
    Task Eliminar(Producto producto);

    IQueryable<Producto> Query();
}
