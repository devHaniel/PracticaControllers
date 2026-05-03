using FluentValidation;
using GestionProducto.Application.Interfaces;
using GestionProducto.Domain.Interfaces;
using GestionProducto.DTOs;
using GestionProducto.DTOs.Producto;
using GestionProducto.Models;

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

        productoExistente.Nombre = dto.Nombre;
        productoExistente.PrecioVenta = dto.PrecioVenta;
        productoExistente.StockMinimo = dto.StockMinimo;
        productoExistente.Activo = dto.Activo;

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

    public async Task<Producto?> ObtenerPorCodigo(string codigo)
    {
        var productoExiste = await _repository.ObtenerPorCodigo(codigo);
        if (productoExiste == null)
            throw new Exception("Producto no encontrado");

        return productoExiste;
    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        var productoExiste = await _repository.ObtenerPorId(id);
        if (productoExiste == null)
            throw new Exception("Producto no encontrado");

        return productoExiste;
    }

    public async Task<int> ObtenerStock(int productoId)
    {
        var productoExiste = await ObtenerPorId(productoId);
        if (productoExiste == null)
            throw new Exception("Producto no encontrado");

        return productoExiste.StockActual;
    }

    public async Task<IEnumerable<ProdutoDto>> ObtenerTodos()
    {
        var productos = await _repository.ObtenerTodos();

        return productos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta,
            StockActual = p.StockActual,
            Activo = p.Activo
        });
    }
}
