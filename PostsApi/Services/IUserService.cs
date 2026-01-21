using PostsApi.DTOs;

namespace PostsApi.Services;

/// <summary>
/// Servicio para lógica de negocio de usuarios
/// </summary>
public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto> CreateUserAsync(string name, string username, string email);
    Task<UserResponseDto> UpdateUserAsync(int id, string name, string username, string email);
    Task<bool> DeleteUserAsync(int id);
}