namespace PostsApi.DTOs;

/// <summary>
/// DTO que exponemos en nuestros endpoints - incluye información enriquecida
/// Razón: No exponemos directamente el modelo de la API externa, 
/// lo que nos permite agregar campos calculados o combinados
/// </summary>
public class PostResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public int WordCount { get; set; }
}