using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Services.Interfaces;
using NatureAPI.Models.Entities;
using Azure;
using Azure.AI.OpenAI;

namespace NatureAPI.Services.Implementations;

public class AiSummaryService : IAiSummaryService
{
    private readonly NatureDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AiSummaryService> _logger;
    private readonly IConfiguration _configuration;
    private readonly OpenAIClient? _openAiClient;

    public AiSummaryService(NatureDbContext context, IHttpClientFactory httpClientFactory, ILogger<AiSummaryService> logger, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
        
        // Inicializar cliente OpenAI si hay API Key
        var apiKey = _configuration["AI:OpenAI:ApiKey"];
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            _openAiClient = new OpenAIClient(apiKey);
        }
    }

    public async Task<string> GeneratePlaceSummaryAsync(int placeId, CancellationToken cancellationToken = default)
    {
        var place = await _context.Places
            .Include(p => p.Trails)
            .Include(p => p.Photos)
            .Include(p => p.PlaceAmenities).ThenInclude(pa => pa.Amenity)
            .FirstOrDefaultAsync(p => p.Id == placeId, cancellationToken);

        if (place == null)
        {
            return "Lugar no encontrado";
        }

        var prompt = $"Genera un resumen breve y atractivo (máx 100 palabras) en español sobre el lugar '{place.Name}', categoría {place.Category}. Incluye: coordenadas aproximadas, elevación {place.ElevationMeters}m, accesibilidad {(place.Accessible ? "sí" : "no")}, costo de entrada {place.EntryFee} MXN. Senderos: {string.Join("; ", place.Trails.Select(t => t.Name + " " + t.DistanceKm + "km"))}. Amenidades: {string.Join(", ", place.PlaceAmenities.Select(a => a.Amenity.Name))}.";

        try
        {
            if (_openAiClient == null)
            {
                _logger.LogWarning("API Key de OpenAI no configurada. Devuelve resumen local.");
                return GenerateLocalFallback(place);
            }

            var model = _configuration["AI:OpenAI:Model"] ?? "gpt-4o-mini";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = model,
                Messages =
                {
                    new ChatRequestSystemMessage("Eres un asistente experto en turismo natural de México. Genera resúmenes atractivos, informativos y concisos de lugares naturales."),
                    new ChatRequestUserMessage(prompt)
                },
                Temperature = 0.7f,
                MaxTokens = 200
            };

            var response = await _openAiClient.GetChatCompletionsAsync(chatCompletionsOptions, cancellationToken);
            var content = response.Value.Choices[0].Message.Content;

            _logger.LogInformation("Resumen IA generado exitosamente para lugar {PlaceId}", placeId);
            return content ?? GenerateLocalFallback(place);
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Error de API OpenAI para lugar {PlaceId}: {Error}", placeId, ex.Message);
            return GenerateLocalFallback(place);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado generando resumen IA para lugar {PlaceId}", placeId);
            return GenerateLocalFallback(place);
        }
    }

    private static string GenerateLocalFallback(Place place)
    {
        return $"{place.Name}: {place.Category}. Elevación {place.ElevationMeters}m. {(place.Accessible ? "Accesible" : "No accesible")}. Entrada {place.EntryFee} MXN. Senderos: {string.Join("; ", place.Trails.Select(t => t.Name + " (" + t.DistanceKm + "km)"))}. Amenidades: {string.Join(", ", place.PlaceAmenities.Select(a => a.Amenity.Name))}.";
    }
}
