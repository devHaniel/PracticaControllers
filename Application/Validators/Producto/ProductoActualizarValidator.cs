using FluentValidation;
using GestionProducto.DTOs.Producto;

namespace GestionProducto.Validators.Producto;

public class ProductoActualizarValidator : AbstractValidator<ProductoActualizarDto>
{
    public ProductoActualizarValidator()
    {
        RuleFor(p => p.Nombre)
        .NotEmpty().WithMessage("El nombre no puede estar vacío.")
        .MaximumLength(50).WithMessage("Máximo de 50 caracteres")
        .When(p => p.Nombre != null);

    RuleFor(p => p.PrecioVenta)
        .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo")
        .When(p => p.PrecioVenta != null);

    RuleFor(p => p.StockMinimo)
        .GreaterThanOrEqualTo(0).WithMessage("Stock mínimo inválido")
        .When(p => p.StockMinimo != null);
    }
}
