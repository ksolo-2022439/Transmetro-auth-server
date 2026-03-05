namespace TransmetroConecta.Auth.Application.DTOs;

public class RegisterRequestDto
{
    public string CUI { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}