using FluentValidation;
using GestionProducto.DTOs.Producto;

namespace GestionProducto.Validators.Producto;

public class ProductoAgregarValidator : AbstractValidator<ProductoAgregarDto>
{
    public ProductoAgregarValidator()
    {
        RuleFor(p => p.Codigo).NotEmpty().WithMessage("El codigo no puede estar vacio.")
                              .MaximumLength(30).WithMessage("Máximo de 30 caracteres.");
        RuleFor(p => p.Nombre).NotEmpty().WithMessage("El nombre no puede estar vacio.")
                              .MaximumLength(50).WithMessage("Máximo de 50 caracteres");
    }
}
