namespace TransmetroConecta.Auth.Application.DTOs;

public class PasswordRecoveryDto
{
    public string Email { get; set; } = string.Empty;
}

public class PasswordResetDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}