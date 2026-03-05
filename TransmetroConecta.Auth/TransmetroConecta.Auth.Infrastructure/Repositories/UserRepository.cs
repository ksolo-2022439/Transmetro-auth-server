using Microsoft.EntityFrameworkCore;
using TransmetroConecta.Auth.Domain.Entities;
using TransmetroConecta.Auth.Domain.Interfaces;
using TransmetroConecta.Auth.Infrastructure.Data;

namespace TransmetroConecta.Auth.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    // Obtiene un usuario por su identificador único.
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }

    // Obtiene un usuario utilizando su CUI.
    public async Task<User?> GetByCuiAsync(string cui)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.CUI == cui);
    }

    // Obtiene un usuario utilizando su dirección de correo electrónico.
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    // Agrega un nuevo usuario a la base de datos.
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    // Actualiza la información de un usuario existente.
    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}