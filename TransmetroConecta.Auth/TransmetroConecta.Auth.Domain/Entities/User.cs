using TransmetroConecta.Auth.Domain.Enums;

namespace TransmetroConecta.Auth.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string CUI { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}