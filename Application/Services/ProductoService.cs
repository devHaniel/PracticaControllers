using FluentValidation;
using GestionProducto.Application.DTOs.Producto;
using GestionProducto.Application.Interfaces;
using GestionProducto.Domain.Interfaces;
using GestionProducto.DTOs;
using GestionProducto.DTOs.Producto;
using GestionProducto.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionProducto.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;
    private readonly IValidator<ProductoAgregarDto> _Agregarvalidator;
    private readonly IValidator<ProductoActualizarDto> _ActualizarValidator;
    public ProductoService(IProductoRepository repository,
    IValidator<ProductoAgregarDto> validator,
    IValidator<ProductoActualizarDto> actualizarValidator)
    {
        _repository = repository;
        _Agregarvalidator = validator;
        _ActualizarValidator = actualizarValidator;
    }

    public async Task Activar(int id)
    {
        var producto = await _repository.ObtenerPorId(id);

        if (producto == null)
            throw new Exception("Producto no encontrado");

        if (producto.Activo)
            throw new Exception("El producto ya está activo");

        producto.Activo = true;

        await _repository.Actualizar(producto);
    }

    public async Task ActualizarProducto(ProductoActualizarDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var validation = await _ActualizarValidator.ValidateAsync(dto);

        if (!validation.IsValid)
            throw new ValidationException(string.Join(", ",
                validation.Errors.Select(e => e.ErrorMessage)));

        var productoExistente = await _repository.ObtenerPorId(dto.Id);

        if (productoExistente == null)
            throw new Exception("Producto no encontrado");

        if (dto.Nombre != null)
            productoExistente.Nombre = dto.Nombre;

        if (dto.PrecioVenta != null)
            productoExistente.PrecioVenta = dto.PrecioVenta.Value;

        if (dto.StockMinimo != null)
            productoExistente.StockMinimo = dto.StockMinimo.Value;

        if (dto.Activo != null)
            productoExistente.Activo = dto.Activo.Value;

        await _repository.Actualizar(productoExistente);
    }

    public async Task<int> CrearProducto(ProductoAgregarDto producto)
    {
        var result = await _Agregarvalidator.ValidateAsync(producto);

        if (!result.IsValid)
            throw new ValidationException(result.Errors.First().ErrorMessage);

        var productoEntity = new Producto()
        {
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            PrecioCompra = producto.PrecioCompra,
            PrecioVenta = producto.PrecioVenta,
            StockMinimo = producto.StockMinimo,
            StockActual = producto.StockActual,
            Activo = producto.Activo
        };

        await _repository.Agregar(productoEntity);
        return productoEntity.Id;

    }


    public async Task EliminarProducto(int id)
    {
        var productoExistente = await _repository.ObtenerPorId(id);

        if (productoExistente == null)
            throw new Exception("Producto no encontrado");

        await _repository.Eliminar(productoExistente);

    }

    public async Task<bool> ExisteCodigo(string codigo)
    {
        var productoExiste = await _repository.ExistePorCodigo(codigo);

        return productoExiste;
    }

    public async Task<IEnumerable<Producto>> Obtener(ProductoFiltroDto producto)
    {
        var query = _repository.Query();

        if (!string.IsNullOrEmpty(producto.Codigo))
            query = query.Where(p => p.Codigo == producto.Codigo);
        if (!string.IsNullOrEmpty(producto.Nombre))
            query = query.Where(p => p.Nombre.Contains(producto.Nombre));
        if (producto.Activo != null)
            query = query.Where(p => p.Activo == producto.Activo);

        return await query.ToListAsync();
    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        var productoExiste = await _repository.ObtenerPorId(id);
        if (productoExiste == null)
            throw new Exception("Producto no encontrado");

        return productoExiste;
    }

    public async Task<IEnumerable<Producto>> ObtenerTodos()
    {
        var productos = await _repository.ObtenerTodos();

        return productos;
    }
}
