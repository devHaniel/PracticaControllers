namespace GestionProducto.Application.Interfaces;

public interface IJwtService
{
    string GenerarToken(int usuarioId, string email, List<string> roles);
}