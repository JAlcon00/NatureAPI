using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureAPI.Models.Entities;

/// <summary>
/// Entidad que representa un lugar natural de México (parque, cascada, mirador, etc.)
/// </summary>
[Table("Places")]
public class Place
{
    /// <summary>
    /// Identificador único del lugar
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nombre del lugar natural
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del lugar
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Categoría del lugar (parque, cascada, mirador, sendero)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Latitud en coordenadas decimales
    /// </summary>
    [Required]
    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90 grados")]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitud en coordenadas decimales
    /// </summary>
    [Required]
    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180 grados")]
    public double Longitude { get; set; }

    /// <summary>
    /// Elevación en metros sobre el nivel del mar
    /// </summary>
    public int ElevationMeters { get; set; }

    /// <summary>
    /// Indica si el lugar es accesible para personas con discapacidad
    /// </summary>
    public bool Accessible { get; set; }

    /// <summary>
    /// Costo de entrada en pesos mexicanos (0 si es gratuito)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "El costo de entrada no puede ser negativo")]
    public double EntryFee { get; set; }

    /// <summary>
    /// Horarios de apertura del lugar
    /// </summary>
    [MaxLength(100)]
    public string OpeningHours { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Propiedades de navegación
    /// <summary>
    /// Senderos asociados al lugar
    /// </summary>
    public virtual ICollection<Trail> Trails { get; set; } = new List<Trail>();

    /// <summary>
    /// Fotografías del lugar
    /// </summary>
    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    /// <summary>
    /// Reseñas de visitantes
    /// </summary>
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    /// <summary>
    /// Amenidades disponibles en el lugar (relación muchos a muchos)
    /// </summary>
    public virtual ICollection<PlaceAmenity> PlaceAmenities { get; set; } = new List<PlaceAmenity>();
}
