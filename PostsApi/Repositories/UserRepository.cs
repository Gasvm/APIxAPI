using PostsApi.Models;
using System.Text.Json;
using System.Text;

namespace PostsApi.Repositories;

/// <summary>
/// Repositorio de Users que consume JSONPlaceholder
/// Usa IHttpClientFactory con cliente nombrado "JsonPlaceholder"
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        IHttpClientFactory httpClientFactory, 
        ILogger<UserRepository> logger)
    {
        // Obtener el cliente nombrado "JsonPlaceholder" (misma config que PostRepository)
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _logger = logger;
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
            _logger.LogError(ex, "Error al obtener usuarios");
            throw new InvalidOperationException("No se pudieron recuperar los usuarios", ex);
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

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("users", content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Error al crear el usuario");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al crear usuario");
            throw new InvalidOperationException("No se pudo crear el usuario", ex);
        }
    }

    public async Task<User> UpdateUserAsync(int id, User user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"users/{id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Error al actualizar el usuario");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario {UserId}", id);
            throw new InvalidOperationException($"No se pudo actualizar el usuario {id}", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario {UserId}", id);
            return false;
        }
    }
}