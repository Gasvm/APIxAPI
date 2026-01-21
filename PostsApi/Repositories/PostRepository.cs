using PostsApi.Models;
using System.Text.Json;
using System.Text;

namespace PostsApi.Repositories;

/// <summary>
/// Repositorio de Posts que consume JSONPlaceholder
/// Cambio clave: Ahora usa IHttpClientFactory con cliente nombrado
/// </summary>
public class PostRepository : IPostRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PostRepository> _logger;

    // CAMBIO IMPORTANTE: Constructor ahora recibe IHttpClientFactory
    public PostRepository(
        IHttpClientFactory httpClientFactory, 
        ILogger<PostRepository> logger)
    {
        // Obtener el cliente nombrado "JsonPlaceholder"
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _logger = logger;
    }

    // ===== MÉTODOS IGUAL QUE ANTES =====
    
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

    public async Task<Post> CreatePostAsync(Post post)
    {
        try
        {
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("posts", content);
            response.EnsureSuccessStatusCode();
            
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
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"posts/{id}", content);
            response.EnsureSuccessStatusCode();
            
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