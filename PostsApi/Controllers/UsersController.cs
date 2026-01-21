using Microsoft.AspNetCore.Mvc;
using PostsApi.Services;
using PostsApi.DTOs;

namespace PostsApi.Controllers;

/// <summary>
/// Controller que expone endpoints REST para gestión de usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPostService _postService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserService userService,
        IPostService postService,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _postService = postService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los usuarios
    /// GET /api/users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los usuarios");
            return StatusCode(500, new { error = "Error al procesar la solicitud" });
        }
    }

    /// <summary>
    /// Obtiene un usuario específico por ID
    /// GET /api/users/{id}
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario {UserId}", id);
            return StatusCode(500, new { error = "Error al procesar la solicitud" });
        }
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

    /// <summary>
    /// Crea un nuevo usuario
    /// POST /api/users
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(dto.Name, dto.Username, dto.Email);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear usuario");
            return StatusCode(500, new { error = "Error al crear el usuario" });
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// PUT /api/users/{id}
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, dto.Name, dto.Username, dto.Email);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Usuario {UserId} no encontrado", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario {UserId}", id);
            return StatusCode(500, new { error = "Error al actualizar el usuario" });
        }
    }

    /// <summary>
    /// Elimina un usuario
    /// DELETE /api/users/{id}
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario {UserId}", id);
            return StatusCode(500, new { error = "Error al eliminar el usuario" });
        }
    }
}