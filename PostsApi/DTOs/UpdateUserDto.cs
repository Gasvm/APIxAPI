namespace PostsApi.DTOs;

/// <summary>
/// DTO para recibir datos al actualizar un usuario existente
/// </summary>
public class UpdateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}