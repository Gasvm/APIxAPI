namespace PostsApi.DTOs;

/// <summary>
/// DTO para recibir datos al actualizar un post existente
/// </summary>
public class UpdatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}