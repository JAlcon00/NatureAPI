using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureAPI.Models.Entities;

/// <summary>
/// Tabla puente para la relación muchos a muchos entre Place y Amenity
/// </summary>
[Table("PlaceAmenities")]
public class PlaceAmenity
{
    /// <summary>
    /// Identificador del lugar
    /// </summary>
    [Required]
    [ForeignKey(nameof(Place))]
    public int PlaceId { get; set; }

    /// <summary>
    /// Identificador de la amenidad
    /// </summary>
    [Required]
    [ForeignKey(nameof(Amenity))]
    public int AmenityId { get; set; }

    // Propiedades de navegación
    /// <summary>
    /// Lugar asociado
    /// </summary>
    public virtual Place Place { get; set; } = null!;

    /// <summary>
    /// Amenidad asociada
    /// </summary>
    public virtual Amenity Amenity { get; set; } = null!;
}
