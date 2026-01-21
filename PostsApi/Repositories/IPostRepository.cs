using PostsApi.Models;

namespace PostsApi.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Posts
/// Razón: Separa responsabilidades, cada entidad tiene su propio repositorio
/// </summary>
public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<Post> CreatePostAsync(Post post);
    Task<Post> UpdatePostAsync(int id, Post post);
    Task<bool> DeletePostAsync(int id);
}