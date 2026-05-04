namespace GestionProducto.Application.DTOs.Usuario;

public class LoginRespuestaDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
