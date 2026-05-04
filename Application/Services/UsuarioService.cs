using GestionProducto.Application.DTOs.Usuario;
using GestionProducto.Application.Interfaces;
using GestionProducto.Domain;
using GestionProducto.Domain.Interfaces;

namespace GestionProducto.Application.Services;


public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRolRepository _rolRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IRolRepository rolRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<UsuarioDto> Crear(UsuarioCrearDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var existeEmail = await _usuarioRepository.ExisteEmail(dto.Email);

        if (existeEmail)
            throw new Exception("Ya existe un usuario con ese email.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Activo = true
        };

        if (dto.Roles == null || !dto.Roles.Any())
            dto.Roles = new List<string> { "Vendedor" };

        foreach (var nombreRol in dto.Roles)
        {
            var rol = await _rolRepository.ObtenerPorNombre(nombreRol);

            if (rol == null)
                throw new Exception($"El rol '{nombreRol}' no existe.");

            usuario.UsuarioRoles.Add(new UsuarioRol
            {
                Usuario = usuario,
                RolId = rol.Id
            });
        }

        await _usuarioRepository.Crear(usuario);

        return MapearUsuarioDto(usuario);
    }

    public async Task<LoginRespuestaDto> Login(LoginDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var usuario = await _usuarioRepository.ObtenerPorEmail(dto.Email);

        if (usuario == null)
            throw new Exception("Credenciales incorrectas.");

        if (!usuario.Activo)
            throw new Exception("El usuario está desactivado.");

        var passwordValida = _passwordHasher.Verify(
            usuario.PasswordHash,
            dto.Password
        );

        if (!passwordValida)
            throw new Exception("Credenciales incorrectas.");

        var roles = usuario.UsuarioRoles
            .Select(ur => ur.Rol.Nombre)
            .ToList();

        var token = _jwtService.GenerarToken(usuario.Id, usuario.Email, roles);

        return new LoginRespuestaDto
        {
            Token = token,
            Email = usuario.Email,
            Roles = roles
        };
    }

    public async Task<IEnumerable<UsuarioDto>> ObtenerTodos()
    {
        var usuarios = await _usuarioRepository.ObtenerTodos();

        return usuarios.Select(MapearUsuarioDto).ToList();
    }

    public async Task<UsuarioDto?> ObtenerPorId(int id)
    {
        var usuario = await _usuarioRepository.ObtenerPorId(id);

        if (usuario == null)
            return null;

        return MapearUsuarioDto(usuario);
    }

    public async Task CambiarEstado(int id, bool activo)
    {
        var usuario = await _usuarioRepository.ObtenerPorId(id);

        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        usuario.Activo = activo;

        await _usuarioRepository.Actualizar(usuario);
    }

    private static UsuarioDto MapearUsuarioDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Activo = usuario.Activo,
            Roles = usuario.UsuarioRoles
                .Select(ur => ur.Rol.Nombre)
                .ToList()
        };
    }
}