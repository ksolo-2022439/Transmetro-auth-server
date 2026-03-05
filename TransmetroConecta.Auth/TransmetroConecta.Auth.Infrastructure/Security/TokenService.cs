using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TransmetroConecta.Auth.Application.Interfaces;
using TransmetroConecta.Auth.Domain.Entities;

namespace TransmetroConecta.Auth.Infrastructure.Security;

public class TokenService(IConfiguration configuration) : ITokenService
{
    // Genera un token JWT para el usuario proporcionado utilizando la clave secreta y la configuración de entorno.
    public string GenerateToken(User user)
    {
        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Falta Jwt:Secret en la configuración.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("cui", user.CUI),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}