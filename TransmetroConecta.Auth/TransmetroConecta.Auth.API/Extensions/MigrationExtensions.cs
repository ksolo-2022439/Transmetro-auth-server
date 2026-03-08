using Microsoft.EntityFrameworkCore;
using TransmetroConecta.Auth.Infrastructure.Data;

namespace TransmetroConecta.Auth.API.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    /// Aplica las migraciones pendientes de Entity Framework a la base de datos y ejecuta la inserción de datos iniciales requeridos para el funcionamiento del sistema.
    /// </summary>
    public static async Task ApplyPendingMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await db.Database.MigrateAsync();
        
        await DataSeeder.SeedAdminAsync(db);
    }
}