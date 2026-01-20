using PostsApi.DTOs;
using PostsApi.Models;
using PostsApi.Repositories;

namespace PostsApi.Services;

/// <summary>
/// Servicio que implementa lógica de negocio y transforma datos
/// </summary>
public class PostService : IPostService
{
    private readonly IJsonPlaceholderRepository _repository;
    private readonly ILogger<PostService> _logger;

    public PostService(
        IJsonPlaceholderRepository repository,
        ILogger<PostService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync()
    {
        var posts = await _repository.GetAllPostsAsync();

        // Obtenemos usuarios para enriquecer los posts
        var users = await _repository.GetAllUsersAsync();
        var userDict = users.ToDictionary(u => u.Id);

        return posts.Select(p => MapToDto(p, userDict.GetValueOrDefault(p.UserId)));
    }

    public async Task<PostResponseDto?> GetPostByIdAsync(int id)
    {
        var post = await _repository.GetPostByIdAsync(id);

        if (post == null)
            return null;

        // Obtenemos el autor del post
        var user = await _repository.GetUserByIdAsync(post.UserId);

        return MapToDto(post, user);
    }

    public async Task<IEnumerable<PostResponseDto>> GetPostsByUserAsync(int userId)
    {
        var posts = await _repository.GetPostsByUserIdAsync(userId);
        var user = await _repository.GetUserByIdAsync(userId);

        return posts.Select(p => MapToDto(p, user));
    }

    public async Task<UserSummaryDto?> GetUserSummaryAsync(int userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);

        if (user == null)
            return null;

        var posts = await _repository.GetPostsByUserIdAsync(userId);

        return new UserSummaryDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TotalPosts = posts.Count() // Lógica de negocio: calculamos estadísticas
        };
    }

    /// <summary>
    /// Transforma el modelo interno a DTO de respuesta
    /// Razón: Centraliza la lógica de transformación y enriquecimiento de datos
    /// </summary>
    private PostResponseDto MapToDto(Post post, User? user)
    {
        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Body,
            AuthorName = user?.Name ?? "Desconocido",
            WordCount = post.Body.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length
        };
    }

    public async Task<PostResponseDto> CreatePostAsync(string title, string content, int userId)
    {
        // 1. Crear el objeto Post a partir de los parámetros
        var post = new Post
        {
            Title = title,
            Body = content,
            UserId = userId
        };

        // 2. Enviar al Repository para crear en JSONPlaceholder
        var createdPost = await _repository.CreatePostAsync(post);

        // 3. Obtener el usuario para enriquecer el DTO
        var user = await _repository.GetUserByIdAsync(userId);

        // 4. Transformar a DTO (con authorName y wordCount)
        return MapToDto(createdPost, user);
    }

    public async Task<PostResponseDto> UpdatePostAsync(int id, string title, string content)
    {
        // 1. Obtener el post existente para verificar que existe
        var existingPost = await _repository.GetPostByIdAsync(id);
        if (existingPost == null)
            throw new InvalidOperationException($"Post {id} no encontrado");

        // 2. Actualizar los campos
        existingPost.Title = title;
        existingPost.Body = content;

        // 3. Enviar al Repository para actualizar en JSONPlaceholder
        var updatedPost = await _repository.UpdatePostAsync(id, existingPost);

        // 4. Obtener el usuario para enriquecer
        var user = await _repository.GetUserByIdAsync(updatedPost.UserId);

        // 5. Transformar a DTO
        return MapToDto(updatedPost, user);
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        // Simplemente delegar al Repository
        return await _repository.DeletePostAsync(id);
    }
}