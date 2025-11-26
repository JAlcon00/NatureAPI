using Microsoft.AspNetCore.Mvc;
using NatureAPI.Models.DTOs;
using NatureAPI.Services.Interfaces;
using FluentValidation;
using OpenAI.Chat;

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
    private readonly IConfiguration _configuration;

    public PlacesController(
        IPlaceService placeService, 
        IValidator<CreatePlaceDto> createPlaceValidator,
        ILogger<PlacesController> logger,
        IConfiguration configuration)
    {
        _placeService = placeService;
        _createPlaceValidator = createPlaceValidator;
        _logger = logger;
        _configuration = configuration;
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
                    e.PropertyName, 
                    e.ErrorMessage 
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

    /// <summary>
    /// Obtiene un resumen de un lugar específico usando IA
    /// </summary>
    /// <param name="id">ID del lugar natural</param>
    /// <param name="aiSummaryService">Servicio de IA para generar resumen</param>
    /// <param name="cancellationToken">Token de cancelación de la operación</param>
    /// <returns>Resumen del lugar</returns>
    /// <response code="200">Resumen obtenido exitosamente</response>
    /// <response code="404">Lugar no encontrado</response>
    [HttpGet("{id:int}/summary")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> GetPlaceSummary(int id, [FromServices] IAiSummaryService aiSummaryService, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GET /api/places/{PlaceId}/summary", id);
        var summary = await aiSummaryService.GeneratePlaceSummaryAsync(id, cancellationToken);
        if (summary == "Lugar no encontrado")
        {
            return NotFound(new { message = summary });
        }
        return Ok(new { placeId = id, summary });
    }

    /// <summary>
    /// Analiza todos los lugares usando IA para obtener insights y recomendaciones
    /// </summary>
    /// <returns>Análisis completo de lugares naturales</returns>
    /// <response code="200">Análisis generado exitosamente</response>
    [HttpGet("ai-analyze")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult> AnalyzePlaces()
    {
        try
        {
            _logger.LogInformation("GET /api/places/ai-analyze - Iniciando análisis con IA");

            // Obtener API Key
            var openAIKey = _configuration["OpenAIKey"];
            if (string.IsNullOrWhiteSpace(openAIKey))
            {
                return BadRequest(new { message = "OpenAI API Key no configurada" });
            }

            var client = new OpenAI.Chat.ChatClient(
                model: "gpt-4o-mini",
                apiKey: openAIKey
            );

            // Obtener todos los lugares básicos primero
            var placesList = await _placeService.GetAllPlacesAsync(null, null);
            
            // Obtener detalles completos de cada lugar
            var placesDetails = new List<PlaceDetailDto>();
            foreach (var place in placesList.Take(10)) // Limitar a 10 lugares para no sobrecargar
            {
                var detail = await _placeService.GetPlaceByIdAsync(place.Id);
                if (detail != null)
                {
                    placesDetails.Add(detail);
                }
            }
            
            // Preparar datos para análisis
            var summary = placesDetails.Select(p => new
            {
                p.Id,
                p.Name,
                p.Category,
                p.ElevationMeters,
                p.Accessible,
                p.EntryFee,
                TrailCount = p.Trails?.Count ?? 0,
                Trails = p.Trails?.Select(t => new { t.Name, t.DistanceKm, t.Difficulty }).ToList(),
                AmenityCount = p.Amenities?.Count ?? 0,
                Amenities = p.Amenities?.Select(a => a.Name).ToList()
            });

            var jsonData = System.Text.Json.JsonSerializer.Serialize(summary, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            // Generar prompt
            var prompt = Services.Prompts.GeneratePlacesAnalysisPrompt(jsonData);

            // Llamar a OpenAI
            var result = await client.CompleteChatAsync([
                new OpenAI.Chat.UserChatMessage(prompt)
            ]);

            // Obtener respuesta
            var response = result.Value.Content[0].Text;

            _logger.LogInformation("Análisis IA completado exitosamente");

            return Ok(new 
            { 
                timestamp = DateTime.UtcNow,
                totalPlacesAnalyzed = placesDetails.Count,
                analysis = response 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al analizar lugares con IA");
            return StatusCode(500, new { message = "Error al realizar análisis con IA", error = ex.Message });
        }
    }
}

