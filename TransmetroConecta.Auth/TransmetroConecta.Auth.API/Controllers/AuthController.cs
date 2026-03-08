using Microsoft.AspNetCore.Mvc;
using TransmetroConecta.Auth.Application.DTOs;
using TransmetroConecta.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TransmetroConecta.Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Procesa la solicitud HTTP para registrar un nuevo usuario en el sistema.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var result = await authService.RegisterAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Procesa la solicitud HTTP para autenticar un usuario y devolver su token JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await authService.LoginAsync(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Expone el endpoint para solicitar el restablecimiento de contraseña enviando el correo electrónico del usuario.
    /// </summary>
    [HttpPost("recover-password")]
    public async Task<IActionResult> RecoverPassword([FromBody] PasswordRecoveryDto request)
    {
        try
        {
            var token = await authService.RequestPasswordResetAsync(request);
            return Ok(new { message = "Token generado exitosamente.", token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Expone el endpoint para confirmar el restablecimiento validando el token y asignando la nueva contraseña.
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto request)
    {
        try
        {
            await authService.ResetPasswordAsync(request);
            return Ok(new { message = "Contraseña actualizada exitosamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene el listado completo de usuarios registrados. Requiere privilegios de Administrador.
    /// </summary>
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await authService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Crea un usuario con rol de administrador. Solo accesible mediante un token de administrador válido.
    /// </summary>
    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDto request)
    {
        try
        {
            var result = await authService.RegisterAdminAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}