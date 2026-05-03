using GestionProducto.Application.DTOs.Producto;
using GestionProducto.DTOs;
using GestionProducto.DTOs.Producto;
using GestionProducto.Models;

namespace GestionProducto.Application.Interfaces;

public interface IProductoService
{
    Task<Producto?> ObtenerPorId(int id);
    Task<IEnumerable<Producto>> ObtenerTodos();
    Task<IEnumerable<Producto>> Obtener(ProductoFiltroDto producto);
    
    Task<int> CrearProducto(ProductoAgregarDto producto);
    Task ActualizarProducto(ProductoActualizarDto producto);
    Task EliminarProducto(int id);

    Task<bool> ExisteCodigo(string codigo);
}
