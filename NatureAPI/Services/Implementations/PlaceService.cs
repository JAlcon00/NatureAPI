using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Models.DTOs;
using NatureAPI.Models.Entities;
using NatureAPI.Services.Interfaces;
using AutoMapper;

namespace NatureAPI.Services.Implementations;

/// <summary>
/// Implementación del servicio para la gestión de lugares naturales
/// </summary>
public class PlaceService : IPlaceService
{
    private readonly NatureDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PlaceService> _logger;

    public PlaceService(NatureDbContext context, IMapper mapper, ILogger<PlaceService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los lugares con filtros opcionales
    /// </summary>
    public async Task<IEnumerable<PlaceListDto>> GetAllPlacesAsync(string? category = null, string? difficulty = null)
    {
        try
        {
            _logger.LogInformation("Obteniendo lugares con filtros: Categoría={Category}, Dificultad={Difficulty}", category, difficulty);

            var query = _context.Places.AsQueryable();

            // Aplicar filtro por categoría
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.ToLower().Contains(category.ToLower()));
            }

            // Aplicar filtro por dificultad de senderos
            if (!string.IsNullOrWhiteSpace(difficulty))
            {
                query = query.Where(p => p.Trails.Any(t => t.Difficulty.ToLower().Contains(difficulty.ToLower())));
            }

            var places = await query
                .OrderBy(p => p.Name)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<PlaceListDto>>(places);
            
            _logger.LogInformation("Se encontraron {Count} lugares", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares");
            throw;
        }
    }

    /// <summary>
    /// Obtiene un lugar específico por su ID con todos sus detalles
    /// </summary>
    public async Task<PlaceDetailDto?> GetPlaceByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo lugar con ID: {PlaceId}", id);

            var place = await _context.Places
                .Include(p => p.Trails)
                .Include(p => p.Photos)
                .Include(p => p.Reviews.OrderByDescending(r => r.CreatedAt))
                .Include(p => p.PlaceAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (place == null)
            {
                _logger.LogWarning("Lugar con ID {PlaceId} no encontrado", id);
                return null;
            }

            var result = _mapper.Map<PlaceDetailDto>(place);
            
            _logger.LogInformation("Lugar {PlaceName} encontrado exitosamente", place.Name);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el lugar con ID: {PlaceId}", id);
            throw;
        }
    }

    /// <summary>
    /// Crea un nuevo lugar natural
    /// </summary>
    public async Task<PlaceDetailDto> CreatePlaceAsync(CreatePlaceDto createPlaceDto)
    {
        try
        {
            _logger.LogInformation("Creando nuevo lugar: {PlaceName}", createPlaceDto.Name);

            // Validar coordenadas de México (aproximadamente)
            if (createPlaceDto.Latitude < 14.5 || createPlaceDto.Latitude > 32.7 ||
                createPlaceDto.Longitude < -118.4 || createPlaceDto.Longitude > -86.7)
            {
                throw new ArgumentException("Las coordenadas deben estar dentro del territorio mexicano");
            }

            var place = _mapper.Map<Place>(createPlaceDto);
            place.CreatedAt = DateTime.UtcNow;

            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Lugar {PlaceName} creado exitosamente con ID: {PlaceId}", place.Name, place.Id);

            // Retornar el lugar creado con todos sus detalles
            return await GetPlaceByIdAsync(place.Id) 
                ?? throw new InvalidOperationException("Error al recuperar el lugar creado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el lugar: {PlaceName}", createPlaceDto.Name);
            throw;
        }
    }

    /// <summary>
    /// Verifica si un lugar existe
    /// </summary>
    public async Task<bool> PlaceExistsAsync(int id)
    {
        try
        {
            return await _context.Places.AnyAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia del lugar con ID: {PlaceId}", id);
            throw;
        }
    }

    /// <summary>
    /// Obtiene estadísticas básicas de los lugares
    /// </summary>
    public async Task<object> GetStatisticsAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo estadísticas de lugares");

            var stats = new
            {
                TotalPlaces = await _context.Places.CountAsync(),
                TotalTrails = await _context.Trails.CountAsync(),
                TotalPhotos = await _context.Photos.CountAsync(),
                TotalReviews = await _context.Reviews.CountAsync(),
                PlacesByCategory = await _context.Places
                    .GroupBy(p => p.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToListAsync(),
                AverageElevation = await _context.Places.AverageAsync(p => (double?)p.ElevationMeters) ?? 0,
                AccessiblePlaces = await _context.Places.CountAsync(p => p.Accessible),
                FreePlaces = await _context.Places.CountAsync(p => p.EntryFee == 0)
            };

            _logger.LogInformation("Estadísticas obtenidas: {TotalPlaces} lugares totales", stats.TotalPlaces);
            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas");
            throw;
        }
    }
}
