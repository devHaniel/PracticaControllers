namespace GestionProducto.Domain;

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    // Relaciones
    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
}