namespace GestionProducto.Domain.Interfaces;


public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorId(int id);
    Task<Usuario?> ObtenerPorEmail(string email);
    Task<IEnumerable<Usuario>> ObtenerTodos();

    Task Crear(Usuario usuario);
    Task Actualizar(Usuario usuario);
    Task Eliminar(Usuario usuario);

    Task<bool> ExisteEmail(string email);
}
