using PostsApi.Models;

namespace PostsApi.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Users
/// </summary>
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
}