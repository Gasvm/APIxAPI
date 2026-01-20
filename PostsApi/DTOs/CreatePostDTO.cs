namespace PostsApi.DTOs;

/// <summary>
/// DTO para recibir datos al crear un nuevo post
/// </summary>
public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
}