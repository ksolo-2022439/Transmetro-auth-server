using TransmetroConecta.Auth.Application.DTOs;

namespace TransmetroConecta.Auth.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Registra un nuevo usuario validando la unicidad del CUI y correo electrónico.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Autentica a un usuario verificando su CUI y contraseña encriptada.
    /// </summary>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<string> RequestPasswordResetAsync(PasswordRecoveryDto request);
    Task ResetPasswordAsync(PasswordResetDto request);

    // Agrega este método a la interfaz existente
    /// <summary>
    /// Obtiene todos los usuarios del sistema mapeados a un formato seguro de transferencia de datos.
    /// </summary>
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<AuthResponseDto> RegisterAdminAsync(RegisterRequestDto request);
}