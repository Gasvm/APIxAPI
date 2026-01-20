namespace PostsApi.DTOs;

/// <summary>
/// DTO que combina datos de usuario con estadísticas calculadas
/// </summary>
public class UserSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TotalPosts { get; set; }
}