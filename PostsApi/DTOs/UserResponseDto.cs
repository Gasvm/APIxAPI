namespace PostsApi.DTOs;

/// <summary>
/// DTO que exponemos para respuestas de usuarios
/// Puedes enriquecerlo con campos calculados si lo necesitas
/// </summary>
public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}