using Microsoft.AspNetCore.Mvc;
using PostsApi.Services;

namespace PostsApi.Controllers;

/// <summary>
/// Controller que expone endpoints REST para información de usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IPostService postService, ILogger<UsersController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene un resumen del usuario con estadísticas de posts
    /// GET /api/users/{id}/summary
    /// </summary>
    [HttpGet("{id}/summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserSummary(int id)
    {
        try
        {
            var summary = await _postService.GetUserSummaryAsync(id);
            
            if (summary == null)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });

            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener resumen del usuario {UserId}", id);
            return StatusCode(500, new { error = "Error al procesar la solicitud" });
        }
    }
}