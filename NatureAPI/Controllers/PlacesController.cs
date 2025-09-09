using Microsoft.AspNetCore.Mvc;
using NatureAPI.Models.DTOs;
using NatureAPI.Services.Interfaces;
using FluentValidation;

namespace NatureAPI.Controllers;

/// <summary>
/// Controlador para la gestión de lugares naturales de México
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlacesController : ControllerBase
{
    private readonly IPlaceService _placeService;
    private readonly IValidator<CreatePlaceDto> _createPlaceValidator;
    private readonly ILogger<PlacesController> _logger;

    public PlacesController(
        IPlaceService placeService, 
        IValidator<CreatePlaceDto> createPlaceValidator,
        ILogger<PlacesController> logger)
    {
        _placeService = placeService;
        _createPlaceValidator = createPlaceValidator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los lugares naturales con filtros opcionales
    /// </summary>
    /// <param name="category">Filtro por categoría (opcional)</param>
    /// <param name="difficulty">Filtro por dificultad de senderos (opcional)</param>
    /// <returns>Lista de lugares naturales</returns>
    /// <response code="200">Lista de lugares obtenida exitosamente</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlaceListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PlaceListDto>>> GetPlaces(
        [FromQuery] string? category = null,
        [FromQuery] string? difficulty = null)
    {
        try
        {
            _logger.LogInformation("GET /api/places - Filtros: categoría={Category}, dificultad={Difficulty}", 
                category, difficulty);

            var places = await _placeService.GetAllPlacesAsync(category, difficulty);
            
            return Ok(places);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares");
            return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un lugar específico por su ID
    /// </summary>
    /// <param name="id">ID del lugar natural</param>
    /// <returns>Detalles completos del lugar</returns>
    /// <response code="200">Lugar encontrado exitosamente</response>
    /// <response code="404">Lugar no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PlaceDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlaceDetailDto>> GetPlace(int id)
    {
        try
        {
            _logger.LogInformation("GET /api/places/{PlaceId}", id);

            var place = await _placeService.GetPlaceByIdAsync(id);
            
            if (place == null)
            {
                _logger.LogWarning("Lugar con ID {PlaceId} no encontrado", id);
                return NotFound(new { message = $"Lugar con ID {id} no encontrado" });
            }

            return Ok(place);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el lugar con ID: {PlaceId}", id);
            return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo lugar natural
    /// </summary>
    /// <param name="createPlaceDto">Datos del lugar a crear</param>
    /// <returns>Lugar creado con todos sus detalles</returns>
    /// <response code="201">Lugar creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(PlaceDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlaceDetailDto>> CreatePlace([FromBody] CreatePlaceDto createPlaceDto)
    {
        try
        {
            _logger.LogInformation("POST /api/places - Creando lugar: {PlaceName}", createPlaceDto.Name);

            // Validar el DTO
            var validationResult = await _createPlaceValidator.ValidateAsync(createPlaceDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { 
                    Property = e.PropertyName, 
                    Error = e.ErrorMessage 
                });
                
                _logger.LogWarning("Validación fallida para el lugar: {PlaceName}. Errores: {@Errors}", 
                    createPlaceDto.Name, errors);
                
                return BadRequest(new { 
                    message = "Datos de entrada inválidos", 
                    errors = errors 
                });
            }

            var createdPlace = await _placeService.CreatePlaceAsync(createPlaceDto);
            
            _logger.LogInformation("Lugar creado exitosamente: {PlaceName} con ID: {PlaceId}", 
                createdPlace.Name, createdPlace.Id);

            return CreatedAtAction(
                nameof(GetPlace), 
                new { id = createdPlace.Id }, 
                createdPlace);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argumentos inválidos al crear lugar: {PlaceName}", createPlaceDto.Name);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el lugar: {PlaceName}", createPlaceDto.Name);
            return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene estadísticas generales de los lugares naturales
    /// </summary>
    /// <returns>Estadísticas de la base de datos</returns>
    /// <response code="200">Estadísticas obtenidas exitosamente</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> GetStatistics()
    {
        try
        {
            _logger.LogInformation("GET /api/places/statistics");

            var statistics = await _placeService.GetStatisticsAsync();
            
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas");
            return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
        }
    }
}
