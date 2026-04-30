using FluentValidation;
using GestionProducto.DTOs.Producto;

namespace GestionProducto.Validators.Producto;

public class ProductoActualizarValidator : AbstractValidator<ProductoActualizarDto>
{
    public ProductoActualizarValidator()
    {
        RuleFor(p => p.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.")
                                .MaximumLength(50).WithMessage("Máximo de 50 caracteres");
        
    }
}
