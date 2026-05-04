using GestionProducto.Application.DTOs.Movimiento;
using GestionProducto.Application.Interfaces;
using GestionProducto.Domain.Interfaces;
using GestionProducto.DTOs.Movimiento;
using GestionProducto.Infra.Persistence;
using GestionProducto.Models;
using GestionProducto.Models.enums;
using Microsoft.EntityFrameworkCore;

namespace GestionProducto.Application.Services;

public class MovimientoService : IMovimientoService
{
    private readonly IMovimientoRepository _repository;
    private readonly IProductoRepository _productoRepository;
    private readonly ApplicationDbContext _dbContext;

    public MovimientoService(
        IMovimientoRepository repository,
        IProductoRepository productoRepository,
        ApplicationDbContext dbContext
        )
    {
        _repository = repository;
        _productoRepository = productoRepository;
        _dbContext = dbContext;
    }
    public async Task<MovimientoDto> Actualizar(MovimientoActualizarDto movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Movimiento no debe ser nulo.");

        var movimientoEncontrado = await _repository.ObtenerPorId(movimiento.Id);

        if (movimientoEncontrado == null)
            throw new Exception("Movimiento no encontrado.");

        movimientoEncontrado.Motivo = movimiento.Motivo ?? movimientoEncontrado.Motivo;

        await _repository.Actualizar(movimientoEncontrado);
        return MapToDto(movimientoEncontrado);
    }

    public async Task<MovimientoDto> Agregar(MovimientoAgregarDto movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Movimiento no debe ser nulo.");

        var productoEncontrado = await _productoRepository.ObtenerPorId(movimiento.ProductoId);

        if (productoEncontrado == null)
            throw new Exception("Producto no existe.");

        if (movimiento.Tipo == TipoMovimiento.Entrada)
        {
            productoEncontrado.StockActual += movimiento.Cantidad;
        }
        else if (movimiento.Tipo == TipoMovimiento.Salida)
        {
            if (productoEncontrado.StockActual < movimiento.Cantidad)
                throw new Exception("Stock insuficiente.");

            productoEncontrado.StockActual -= movimiento.Cantidad;
        }

        var movimientoResult = new Movimiento()
        {
            ProductoId = movimiento.ProductoId,
            Tipo = movimiento.Tipo,
            Motivo = movimiento.Motivo,
            Cantidad = movimiento.Cantidad,
            Fecha = DateTime.UtcNow
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await _repository.Agregar(movimientoResult);
            await _productoRepository.Actualizar(productoEncontrado);
            await transaction.CommitAsync();
            return MapToDto(movimientoResult);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task Eliminar(int id)
    {
        var movimientoEncontrado = await _repository.ObtenerPorId(id);

        if (movimientoEncontrado == null)
            throw new Exception("Movimiento no encontrado.");

        await _repository.Eliminar(movimientoEncontrado);
    }


    public async Task<MovimientoDto?> ObtenerPorId(int id)
    {
        var movimiento = await _repository.ObtenerPorId(id);

        if (movimiento == null)
            return null;

        return MapToDto(movimiento);
    }

    public async Task<IEnumerable<MovimientoDto>> ObtenerPorIdProducto(int id)
    {
        return (await _repository.ObtenerPorIdProducto(id))
            .Select(MapToDto);
    }

    public async Task<IEnumerable<MovimientoDto>> Obtener(MovimientoFiltroDto filtro)
    {
        var query = _repository.Query();

        if (filtro.ProductoId.HasValue)
            query = query.Where(m => m.ProductoId == filtro.ProductoId);

        if (filtro.Tipo.HasValue)
            query = query.Where(m => m.Tipo == filtro.Tipo);

        if (filtro.FechaInicio.HasValue)
            query = query.Where(m => m.Fecha >= filtro.FechaInicio);

        if (filtro.FechaFin.HasValue)
            query = query.Where(m => m.Fecha <= filtro.FechaFin);

        if (!string.IsNullOrEmpty(filtro.Motivo))
            query = query.Where(m => m.Motivo.Contains(filtro.Motivo));

        return (await query.ToListAsync()).Select(MapToDto);
    }

    public async Task<IEnumerable<MovimientoDto>> ObtenerTodos()
    {
        return (await _repository.ObtenerTodos()).Select(MapToDto);
    }

    private static MovimientoDto MapToDto(Movimiento movimiento)
    {
        return new MovimientoDto
        {
            Id = movimiento.Id,
            ProductoId = movimiento.ProductoId,
            Tipo = movimiento.Tipo,
            Motivo = movimiento.Motivo,
            Cantidad = movimiento.Cantidad,
            Fecha = movimiento.Fecha
        };
    }
}
