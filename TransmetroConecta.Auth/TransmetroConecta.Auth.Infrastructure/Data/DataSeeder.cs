using Microsoft.EntityFrameworkCore;
using TransmetroConecta.Auth.Domain.Entities;
using TransmetroConecta.Auth.Domain.Enums;

namespace TransmetroConecta.Auth.Infrastructure.Data;

public static class DataSeeder
{
    /// <summary>
    /// Verifica la existencia de un usuario con rol de Administrador en la base de datos y lo crea con credenciales predeterminadas si no existe.
    /// </summary>
    public static async Task SeedAdminAsync(AppDbContext context)
    {
        var adminExists = await context.Users.AnyAsync(u => u.Role == Role.Admin);

        if (!adminExists)
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                CUI = "0000000000000",
                Email = "admin@transmetro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminTransmetro2026!"),
                Role = Role.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}