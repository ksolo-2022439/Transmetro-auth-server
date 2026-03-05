using TransmetroConecta.Auth.Application.DTOs;

namespace TransmetroConecta.Auth.Application.Interfaces;

public interface IAuthService
{
    /// Registra un nuevo usuario validando la unicidad del CUI y correo electrónico.
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    /// Autentica a un usuario verificando su CUI y contraseña encriptada.
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}