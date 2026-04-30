using GestionProducto.DTOs;
using GestionProducto.DTOs.Producto;
using GestionProducto.Models;

namespace GestionProducto.Application.Interfaces;

public interface IProductoService
{
    Task<Producto?> ObtenerPorId(int id);
    Task<IEnumerable<ProdutoDto>> ObtenerTodos();
    Task<Producto?> ObtenerPorCodigo(string codigo);

    Task<int> CrearProducto(ProductoAgregarDto producto);
    Task ActualizarProducto(ProductoActualizarDto producto);
    Task EliminarProducto(int id);

    Task<int> ObtenerStock(int productoId);
    Task AgregarStock(int productoId, int cantidad);
    Task DisminuirStock(int productoId, int cantidad);

    Task<bool> ExisteCodigo(string codigo);
}
