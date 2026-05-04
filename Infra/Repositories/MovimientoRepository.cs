using GestionProducto.Domain.Interfaces;
using GestionProducto.Infra.Persistence;
using GestionProducto.Models;
using GestionProducto.Models.enums;
using Microsoft.EntityFrameworkCore;

namespace GestionProducto.Infra.Repositories;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MovimientoRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    public async Task Actualizar(Movimiento movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Invalido.");


        var movimientoDB = await _dbContext.Movimientos
            .FirstOrDefaultAsync(p => p.Id == movimiento.Id);

        if (movimientoDB == null)
            return;

        movimientoDB.Motivo = movimiento.Motivo;

        _dbContext.Update(movimientoDB);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Agregar(Movimiento movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Movimiento nulo.");

        await _dbContext.Movimientos.AddAsync(movimiento);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Eliminar(Movimiento movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Invalido.");

        _dbContext.Movimientos.Remove(movimiento);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Movimiento?> ObtenerPorId(int id)
    {
        return await _dbContext.Movimientos
        .Include(m => m.Producto)
        .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movimiento>> ObtenerPorIdProducto(int id)
    {
        return await _dbContext.Movimientos
        .Include(m => m.Producto)
        .Where(m => m.ProductoId == id)
        .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> ObtenerTodos()
    {
        return await _dbContext.Movimientos
        .Include(m => m.Producto)
        .ToListAsync();
    }

    public IQueryable<Movimiento> Query()
    {
        return _dbContext.Movimientos;
    }
}
