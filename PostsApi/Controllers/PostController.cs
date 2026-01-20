using Microsoft.AspNetCore.Mvc;
using PostsApi.Services;

namespace PostsApi.Controllers;

/// <summary>
/// Controller que expone endpoints REST para gestión de posts
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostService postService, ILogger<PostsController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los posts con información del autor
    /// GET /api/posts
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los posts");
            return StatusCode(500, new { error = "Error al procesar la solicitud" });
        }
    }

    /// <summary>
    /// Obtiene un post específico por ID
    /// GET /api/posts/{id}
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostById(int id)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id);
            
            if (post == null)
                return NotFound(new { message = $"Post con ID {id} no encontrado" });

            return Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener post {PostId}", id);
            return StatusCode(500, new { error = "Error al procesar la solicitud" });
        }
    }

    /// <summary>
    /// Obtiene todos los posts de un usuario específico
    /// GET /api/posts/user/{userId}
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostsByUser(int userId)
    {
        try
        {
            var posts = await _postService.GetPostsByUserAsync(userId);
            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener posts del usuario {UserId}", userId);
            return StatusCode(500, new { error = "Error al procesar la solicitud" });
        }
    }
}