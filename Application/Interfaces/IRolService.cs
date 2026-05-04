using GestionProducto.Application.DTOs.Rol;

namespace GestionProducto.Application.Interfaces;

public interface IRolService
{
    Task<IEnumerable<RolDto>> ObtenerTodos();
}