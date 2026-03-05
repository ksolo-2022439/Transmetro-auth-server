using FluentValidation;
using TransmetroConecta.Auth.Application.DTOs;

namespace TransmetroConecta.Auth.Application.Validators;

public class TransactionRequestDtoValidator : AbstractValidator<TransactionRequestDto>
{
    /// <summary>
    /// Configura las reglas de validación para procesar una recarga de saldo, verificando la longitud de la tarjeta, el formato del CVV y exigiendo un monto positivo.
    /// </summary>
    public TransactionRequestDtoValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("El número de tarjeta es requerido.")
            .MinimumLength(15).MaximumLength(19).WithMessage("La longitud de la tarjeta es inválida.");

        RuleFor(x => x.CVV)
            .NotEmpty().WithMessage("El CVV es requerido.")
            .Length(3, 4).WithMessage("El CVV debe tener entre 3 y 4 dígitos.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto de recarga debe ser mayor a cero.");
    }
}