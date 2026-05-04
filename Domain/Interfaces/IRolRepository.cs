namespace GestionProducto.Domain.Interfaces;


public interface IRolRepository
{
    Task<Rol?> ObtenerPorId(int id);
    Task<Rol?> ObtenerPorNombre(string nombre);
    Task<IEnumerable<Rol>> ObtenerTodos();
}