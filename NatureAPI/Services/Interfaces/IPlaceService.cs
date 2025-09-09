using NatureAPI.Models.DTOs;

namespace NatureAPI.Services.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de lugares naturales
/// </summary>
public interface IPlaceService
{
    /// <summary>
    /// Obtiene todos los lugares con filtros opcionales
    /// </summary>
    /// <param name="category">Filtro por categoría (opcional)</param>
    /// <param name="difficulty">Filtro por dificultad de senderos (opcional)</param>
    /// <returns>Lista de lugares naturales</returns>
    Task<IEnumerable<PlaceListDto>> GetAllPlacesAsync(string? category = null, string? difficulty = null);

    /// <summary>
    /// Obtiene un lugar específico por su ID con todos sus detalles
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <returns>Detalles completos del lugar o null si no existe</returns>
    Task<PlaceDetailDto?> GetPlaceByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo lugar natural
    /// </summary>
    /// <param name="createPlaceDto">Datos del lugar a crear</param>
    /// <returns>Detalles del lugar creado</returns>
    Task<PlaceDetailDto> CreatePlaceAsync(CreatePlaceDto createPlaceDto);

    /// <summary>
    /// Verifica si un lugar existe
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    Task<bool> PlaceExistsAsync(int id);

    /// <summary>
    /// Obtiene estadísticas básicas de los lugares
    /// </summary>
    /// <returns>Estadísticas de la base de datos</returns>
    Task<object> GetStatisticsAsync();
}
