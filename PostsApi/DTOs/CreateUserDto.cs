namespace PostsApi.DTOs;

/// <summary>
/// DTO para recibir datos al crear un nuevo usuario
/// </summary>
public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}