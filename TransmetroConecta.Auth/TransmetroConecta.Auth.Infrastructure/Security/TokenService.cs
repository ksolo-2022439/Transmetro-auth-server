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

    /// Genera un token JWT de 15 minutos exclusivo para el proceso de restablecimiento de contraseña.
    public string GeneratePasswordResetToken(User user)
    {
        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Falta Jwt:Secret");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Purpose", "PasswordReset")
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// Valida la firma, expiración y propósito del token temporal proporcionado para la recuperación.
    public bool ValidatePasswordResetToken(string token, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Falta Jwt:Secret");
        var key = Encoding.UTF8.GetBytes(secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var tokenEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var purpose = jwtToken.Claims.First(x => x.Type == "Purpose").Value;

            return tokenEmail == email && purpose == "PasswordReset";
        }
        catch
        {
            return false;
        }
    }
}