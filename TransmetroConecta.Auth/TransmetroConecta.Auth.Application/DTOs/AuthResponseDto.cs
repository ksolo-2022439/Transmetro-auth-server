namespace TransmetroConecta.Auth.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Role { get; set; } = string.Empty;
}