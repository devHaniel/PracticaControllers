using GestionProducto.Application.DTOs.Rol;
using GestionProducto.Application.Interfaces;
using GestionProducto.Domain.Interfaces;

namespace GestionProducto.Application.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;

    public RolService(IRolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    public async Task<IEnumerable<RolDto>> ObtenerTodos()
    {
        var roles = await _rolRepository.ObtenerTodos();

        return roles.Select(r => new RolDto
        {
            Id = r.Id,
            Nombre = r.Nombre
        });
    }
}