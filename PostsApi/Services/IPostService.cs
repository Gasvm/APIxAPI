using PostsApi.DTOs;

namespace PostsApi.Services;

/// <summary>
/// Contrato del servicio que contiene lógica de negocio
/// Razón: Separa lógica de negocio de presentación (Controllers) y acceso a datos (Repositories)
/// </summary>
public interface IPostService
{
    Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
    Task<PostResponseDto?> GetPostByIdAsync(int id);
    Task<IEnumerable<PostResponseDto>> GetPostsByUserAsync(int userId);
    Task<UserSummaryDto?> GetUserSummaryAsync(int userId);
    Task<PostResponseDto> CreatePostAsync(string title, string content, int userId);
    Task<PostResponseDto> UpdatePostAsync(int id, string title, string content);
    Task<bool> DeletePostAsync(int id);
}