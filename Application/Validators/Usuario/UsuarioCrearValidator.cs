using FluentValidation;
using GestionProducto.Application.DTOs.Usuario;
namespace GestionProducto.Application.Validators.Usuario;

public class UsuarioCrearValidator : AbstractValidator<UsuarioCrearDto>
{
    public UsuarioCrearValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("El email no tiene un formato válido.")
            .MaximumLength(150).WithMessage("El email no puede superar los 150 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .MaximumLength(100).WithMessage("La contraseña no puede superar los 100 caracteres.");

        RuleFor(x => x.Roles)
            .NotNull().WithMessage("La lista de roles no puede ser nula.");

        RuleForEach(x => x.Roles)
            .NotEmpty().WithMessage("El rol no puede estar vacío.")
            .MaximumLength(50).WithMessage("El rol no puede superar los 50 caracteres.");
    }
}
