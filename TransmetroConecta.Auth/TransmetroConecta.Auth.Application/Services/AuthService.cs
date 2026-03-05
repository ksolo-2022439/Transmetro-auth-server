using TransmetroConecta.Auth.Application.DTOs;
using TransmetroConecta.Auth.Application.Interfaces;
using TransmetroConecta.Auth.Domain.Entities;
using TransmetroConecta.Auth.Domain.Enums;
using TransmetroConecta.Auth.Domain.Interfaces;

namespace TransmetroConecta.Auth.Application.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    /// <summary>
    /// Registra un nuevo usuario validando la unicidad del CUI y correo electrónico.
    /// </summary>
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await userRepository.GetByCuiAsync(request.CUI);
        if (existingUser != null) throw new Exception("El CUI ya está registrado.");

        var emailExists = await userRepository.GetByEmailAsync(request.Email);
        if (emailExists != null) throw new Exception("El correo ya está en uso.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            CUI = request.CUI,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.User
        };

        await userRepository.AddAsync(user);

        var token = tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Role = user.Role.ToString()
        };
    }

    /// <summary>
    /// Autentica a un usuario verificando su CUI y contraseña encriptada.
    /// </summary>
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await userRepository.GetByCuiAsync(request.CUI);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        var token = tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Role = user.Role.ToString()
        };
    }

    /// <summary>
    /// Verifica la existencia del usuario por correo electrónico y emite un token temporal de recuperación.
    /// </summary>
    public async Task<string> RequestPasswordResetAsync(PasswordRecoveryDto request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null) throw new Exception("Usuario no encontrado.");
        
        return tokenService.GeneratePasswordResetToken(user);
    }

    /// <summary>
    /// Valida el token temporal y procesa la actualización de la contraseña encriptada en la base de datos.
    /// </summary>
    public async Task ResetPasswordAsync(PasswordResetDto request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null) throw new Exception("Usuario no encontrado.");

        if (!tokenService.ValidatePasswordResetToken(request.Token, request.Email))
            throw new UnauthorizedAccessException("Token inválido o expirado.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await userRepository.UpdateAsync(user);
    }
}