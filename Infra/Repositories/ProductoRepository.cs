using GestionProducto.Domain.Interfaces;
using GestionProducto.Infra.Persistence;
using GestionProducto.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionProducto.Infra.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ProductoRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Actualizar(Producto producto)
    {
        if (producto == null)
            return;

        var productoDB = await _dbContext.Productos
            .FirstOrDefaultAsync(p => p.Id == producto.Id);

        if (productoDB == null)
            return;

        productoDB.Nombre = producto.Nombre;
        productoDB.Codigo = producto.Codigo;
        productoDB.StockActual = producto.StockActual;
        productoDB.StockMinimo = producto.StockMinimo;
        productoDB.PrecioCompra = producto.PrecioCompra;
        productoDB.PrecioVenta = producto.PrecioVenta;

        await _dbContext.SaveChangesAsync();
    }

    public async Task Agregar(Producto producto)
    {
        if (producto != null)
        {
            await _dbContext.Productos.AddAsync(producto);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task Eliminar(Producto producto)
    {

        if (producto == null)
            return;

        _dbContext.Productos.Remove(producto);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistePorCodigo(string codigo)
    {
        return await _dbContext.Productos.AnyAsync(p => p.Codigo == codigo);
    }


    public async Task<Producto?> ObtenerPorCodigo(string codigo)
    {
        return await _dbContext.Productos.FirstOrDefaultAsync(p => p.Codigo == codigo);

    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        return await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> ObtenerStock(int id)
    {
        return await _dbContext.Productos
            .Where(p => p.Id == id)
            .Select(p => p.StockActual)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Producto>> ObtenerTodos()
    {
        return await _dbContext.Productos.ToListAsync();
    }
}
