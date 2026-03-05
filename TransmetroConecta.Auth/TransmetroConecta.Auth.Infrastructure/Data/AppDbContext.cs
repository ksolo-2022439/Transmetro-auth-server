using Microsoft.EntityFrameworkCore;
using TransmetroConecta.Auth.Domain.Entities;

namespace TransmetroConecta.Auth.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    /// <summary>
    // Configura el modelo de datos de la base de datos definiendo llaves primarias y restricciones únicas.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CUI).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}