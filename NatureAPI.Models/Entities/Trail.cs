using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureAPI.Models.Entities;

/// <summary>
/// Entidad que representa un sendero o ruta dentro de un lugar natural
/// </summary>
[Table("Trails")]
public class Trail
{
    /// <summary>
    /// Identificador único del sendero
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identificador del lugar al que pertenece el sendero
    /// </summary>
    [Required]
    [ForeignKey(nameof(Place))]
    public int PlaceId { get; set; }

    /// <summary>
    /// Nombre del sendero
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Distancia del sendero en kilómetros
    /// </summary>
    [Required]
    [Range(0.1, 1000, ErrorMessage = "La distancia debe ser entre 0.1 y 1000 km")]
    public double DistanceKm { get; set; }

    /// <summary>
    /// Tiempo estimado de recorrido en minutos
    /// </summary>
    [Required]
    [Range(5, 10080, ErrorMessage = "El tiempo estimado debe ser entre 5 minutos y 7 días")]
    public int EstimatedTimeMinutes { get; set; }

    /// <summary>
    /// Nivel de dificultad del sendero (Fácil, Moderado, Difícil, Extremo)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Difficulty { get; set; } = string.Empty;

    /// <summary>
    /// Coordenadas del recorrido en formato JSON o texto
    /// </summary>
    [MaxLength(2000)]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el sendero es un circuito cerrado
    /// </summary>
    public bool IsLoop { get; set; }

    // Propiedades de navegación
    /// <summary>
    /// Lugar al que pertenece el sendero
    /// </summary>
    public virtual Place Place { get; set; } = null!;
}
