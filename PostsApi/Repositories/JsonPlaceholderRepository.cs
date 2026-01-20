using PostsApi.Models;
using System.Text.Json;

namespace PostsApi.Repositories;

/// <summary>
/// Implementación que consume la API JSONPlaceholder
/// </summary>
public class JsonPlaceholderRepository : IJsonPlaceholderRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<JsonPlaceholderRepository> _logger;

    public JsonPlaceholderRepository(
        HttpClient httpClient, 
        ILogger<JsonPlaceholderRepository> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("posts");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Post>>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
                ?? Enumerable.Empty<Post>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al obtener posts de la API externa");
            throw new InvalidOperationException("No se pudieron recuperar los posts", ex);
        }
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"posts/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Post>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al obtener post {PostId}", id);
            throw new InvalidOperationException($"No se pudo recuperar el post {id}", ex);
        }
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"posts?userId={userId}");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Post>>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
                ?? Enumerable.Empty<Post>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al obtener posts del usuario {UserId}", userId);
            throw new InvalidOperationException($"No se pudieron recuperar los posts del usuario {userId}", ex);
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al obtener usuario {UserId}", id);
            throw new InvalidOperationException($"No se pudo recuperar el usuario {id}", ex);
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<User>>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
                ?? Enumerable.Empty<User>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios de la API externa");
            throw new InvalidOperationException("No se pudieron recuperar los usuarios", ex);
        }
    }
}