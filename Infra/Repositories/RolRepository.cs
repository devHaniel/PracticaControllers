using GestionProducto.Domain;
using GestionProducto.Domain.Interfaces;
using GestionProducto.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionProducto.Infra.Repositories;

public class RolRepository : IRolRepository
{
    private readonly ApplicationDbContext _context;

    public RolRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Rol?> ObtenerPorId(int id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rol?> ObtenerPorNombre(string nombre)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Nombre == nombre);
    }

    public async Task<IEnumerable<Rol>> ObtenerTodos()
    {
        return await _context.Roles
            .ToListAsync();
    }
}