using PostsApi.DTOs;
using PostsApi.Models;
using PostsApi.Repositories;

namespace PostsApi.Services;

/// <summary>
/// Servicio que implementa lógica de negocio para usuarios
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        if (user == null)
            return null;

        return MapToDto(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(string name, string username, string email)
    {
        // 1. Construir el objeto User
        var user = new User
        {
            Name = name,
            Username = username,
            Email = email
        };

        // 2. Enviar al Repository para crear en JSONPlaceholder
        var createdUser = await _userRepository.CreateUserAsync(user);

        // 3. Transformar a DTO
        return MapToDto(createdUser);
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, string name, string username, string email)
    {
        // 1. Verificar que el usuario existe
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
            throw new InvalidOperationException($"Usuario {id} no encontrado");

        // 2. Actualizar los campos
        existingUser.Name = name;
        existingUser.Username = username;
        existingUser.Email = email;

        // 3. Enviar al Repository para actualizar
        var updatedUser = await _userRepository.UpdateUserAsync(id, existingUser);

        // 4. Transformar a DTO
        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        // Delegar directamente al Repository
        return await _userRepository.DeleteUserAsync(id);
    }

    /// <summary>
    /// Transforma Model User a DTO
    /// Por ahora es mapeo 1:1, pero podrías añadir lógica
    /// </summary>
    private UserResponseDto MapToDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Email = user.Email
        };
    }
}