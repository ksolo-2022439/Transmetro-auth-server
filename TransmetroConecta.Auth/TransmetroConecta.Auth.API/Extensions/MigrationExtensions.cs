using Microsoft.EntityFrameworkCore;
using TransmetroConecta.Auth.Infrastructure.Data;

namespace TransmetroConecta.Auth.API.Extensions;

public static class MigrationExtensions
{
    // Aplica las migraciones pendientes a la base de datos al iniciar la aplicación.
    public static void ApplyPendingMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}