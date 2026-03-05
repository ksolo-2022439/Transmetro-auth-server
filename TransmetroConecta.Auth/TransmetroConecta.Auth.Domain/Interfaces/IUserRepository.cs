using TransmetroConecta.Auth.Domain.Entities;

namespace TransmetroConecta.Auth.Domain.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Obtiene un usuario por su identificador único.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene un usuario utilizando su CUI.
    /// </summary>
    Task<User?> GetByCuiAsync(string cui);

    /// <summary>
    /// Obtiene un usuario utilizando su dirección de correo electrónico.
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Agrega un nuevo usuario a la base de datos.
    /// </summary>
    Task AddAsync(User user);

    /// <summary>
    /// Actualiza la información de un usuario existente.
    /// </summary>
    Task UpdateAsync(User user);
}