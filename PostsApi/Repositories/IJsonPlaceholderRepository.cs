using PostsApi.Models;

namespace PostsApi.Repositories;

/// <summary>
/// Contrato para acceso a datos de la API externa
/// Razón: Permite cambiar la implementación (otra API, BD, cache) 
/// sin afectar capas superiores
/// </summary>
public interface IJsonPlaceholderRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
}