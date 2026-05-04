using GestionProducto.Application.DTOs.Usuario;

namespace GestionProducto.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto> Crear(UsuarioCrearDto dto);
    Task<LoginRespuestaDto> Login(LoginDto dto);
    Task<IEnumerable<UsuarioDto>> ObtenerTodos();
    Task<UsuarioDto?> ObtenerPorId(int id);
    Task CambiarEstado(int id, bool activo);
}