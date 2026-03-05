using Microsoft.AspNetCore.Mvc;
using TransmetroConecta.Auth.Application.DTOs;
using TransmetroConecta.Auth.Application.Interfaces;

namespace TransmetroConecta.Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    // Procesa la solicitud HTTP para registrar un nuevo usuario en el sistema.
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

    // Procesa la solicitud HTTP para autenticar un usuario y devolver su token JWT.
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

    /// Expone el endpoint para solicitar el restablecimiento de contraseña enviando el correo electrónico del usuario.
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

    /// Expone el endpoint para confirmar el restablecimiento validando el token y asignando la nueva contraseña.
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
}