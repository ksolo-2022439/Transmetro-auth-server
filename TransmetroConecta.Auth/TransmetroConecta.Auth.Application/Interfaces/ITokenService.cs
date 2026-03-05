using TransmetroConecta.Auth.Domain.Entities;

namespace TransmetroConecta.Auth.Application.Interfaces;

public interface ITokenService
{
    /// Genera un token JWT para el usuario proporcionado.
    string GenerateToken(User user);
    string GeneratePasswordResetToken(User user);
    bool ValidatePasswordResetToken(string token, string email);
}