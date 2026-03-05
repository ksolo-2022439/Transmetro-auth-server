using TransmetroConecta.Auth.Domain.Entities;

namespace TransmetroConecta.Auth.Domain.Interfaces;

public interface IUserRepository
{
    /// Obtiene un usuario por su identificador único.
    Task<User?> GetByIdAsync(Guid id);

    /// Obtiene un usuario utilizando su CUI.
    Task<User?> GetByCuiAsync(string cui);

    /// Obtiene un usuario utilizando su dirección de correo electrónico.
    Task<User?> GetByEmailAsync(string email);

    /// Agrega un nuevo usuario a la base de datos.
    Task AddAsync(User user);

    /// Actualiza la información de un usuario existente.
    Task UpdateAsync(User user);
}