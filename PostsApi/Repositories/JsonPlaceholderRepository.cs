using PostsApi.Models;
using System.Text.Json;
using System.Text;

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

    public async Task<Post> CreatePostAsync(Post post)
    {
        try
        {
            // Serializar el post a JSON
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Hacer POST a la API
            var response = await _httpClient.PostAsync("posts", content);
            response.EnsureSuccessStatusCode();

            // Deserializar la respuesta
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Post>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Error al crear el post");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al crear post");
            throw new InvalidOperationException("No se pudo crear el post", ex);
        }
    }

    public async Task<Post> UpdatePostAsync(int id, Post post)
    {
        try
        {
            // Serializar el post a JSON
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Hacer PUT a la API
            var response = await _httpClient.PutAsync($"posts/{id}", content);
            response.EnsureSuccessStatusCode();

            // Deserializar la respuesta
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Post>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Error al actualizar el post");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al actualizar post {PostId}", id);
            throw new InvalidOperationException($"No se pudo actualizar el post {id}", ex);
        }
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        try
        {
            // Hacer DELETE a la API
            var response = await _httpClient.DeleteAsync($"posts/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al eliminar post {PostId}", id);
            return false;
        }
    }
}