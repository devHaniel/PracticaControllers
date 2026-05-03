using GestionProducto.Application.DTOs.Movimiento;
using GestionProducto.Application.Interfaces;
using GestionProducto.Domain.Interfaces;
using GestionProducto.DTOs.Movimiento;
using GestionProducto.Models;
using GestionProducto.Models.enums;

namespace GestionProducto.Application.Services;

public class MovimientoService : IMovimientoService
{
    private readonly IMovimientoRepository _repository;
    private readonly IProductoService _productoService;
    public MovimientoService(
        IMovimientoRepository repository,
        IProductoService productoService
        )
    {
        _repository = repository;
        _productoService = productoService;
    }
    public async Task Actualizar(MovimientoActualizarDto movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Movimiento no debe ser nulo.");

        var movimientoEncontrado = await _repository.ObtenerPorId(movimiento.Id);

        if (movimientoEncontrado == null)
            throw new Exception("Movimiento no encontrado.");

        movimientoEncontrado.Motivo = movimiento.Motivo ?? movimientoEncontrado.Motivo;

        await _repository.Actualizar(movimientoEncontrado);
    }

    public async Task Agregar(MovimientoAgregarDto movimiento)
    {
        if (movimiento == null)
            throw new ArgumentNullException("Movimiento no debe ser nulo.");

        var productoEncontrado = await _productoService.ObtenerPorId(movimiento.ProductoId);

        if (productoEncontrado == null)
            throw new Exception("Producto no existe.");

        try
        {
            var movimientoResult = new Movimiento()
            {
                ProductoId = movimiento.ProductoId,
                Tipo = movimiento.Tipo,
                Motivo = movimiento.Motivo,
                Cantidad = movimiento.Cantidad,
                Fecha = DateTime.UtcNow
            };

            await _repository.Agregar(movimientoResult);
        }
        catch
        {
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

    public async Task<IEnumerable<Movimiento>> ObtenerPorFecha(DateTime fecha)
    {
        if (fecha == DateTime.MinValue)
            throw new ArgumentException("La fechan no es valida.");


        var movimiento = await _repository.ObtenerPorFecha(fecha);

        if (movimiento == null)
        {
            throw new Exception("Movimiento no encontrado.");
        }

        return movimiento;
    }

    public async Task<Movimiento?> ObtenerPorId(int id)
    {
        var movimiento = await _repository.ObtenerPorId(id);

        if (movimiento == null)
            throw new Exception("Movimiento no encontrado.");

        return movimiento;
    }

    public async Task<IEnumerable<MovimientoDto>> ObtenerPorIdProducto(int id)
    {
        return (await _repository.ObtenerPorIdProducto(id))
        .Select(m => new MovimientoDto
        {
            Id = m.Id,
            ProductoId = m.ProductoId,
            Tipo = m.Tipo,
            Motivo = m.Motivo,
            Cantidad = m.Cantidad,
            Fecha = m.Fecha
        });
    }

    public async Task<IEnumerable<Movimiento>> Obtener(MovimientoFiltroDto filtro)
    {
        var query = await _repository.ObtenerTodos();

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

        return query.ToList();
    }

    public async Task<IEnumerable<Movimiento>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }
}
