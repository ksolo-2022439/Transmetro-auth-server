using FluentValidation;
using TransmetroConecta.Auth.Application.DTOs;

namespace TransmetroConecta.Auth.Application.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    /// <summary>
    /// Configura las reglas de validación para el registro de un nuevo usuario, asegurando el formato exacto de 13 dígitos del CUI, un correo válido y la fortaleza de la contraseña.
    /// </summary>
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.CUI)
            .NotEmpty().WithMessage("El CUI es requerido.")
            .Length(13).WithMessage("El CUI debe tener exactamente 13 dígitos.")
            .Matches("^[0-9]+$").WithMessage("El CUI solo debe contener números.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido.")
            .EmailAddress().WithMessage("El formato del correo electrónico es inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }
}