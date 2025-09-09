using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureAPI.Models.Entities;

/// <summary>
/// Entidad que representa una amenidad o servicio disponible en los lugares naturales
/// </summary>
[Table("Amenities")]
public class Amenity
{
    /// <summary>
    /// Identificador único de la amenidad
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la amenidad (baños, estacionamiento, mirador, etc.)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Propiedades de navegación
    /// <summary>
    /// Lugares que tienen esta amenidad (relación muchos a muchos)
    /// </summary>
    public virtual ICollection<PlaceAmenity> PlaceAmenities { get; set; } = new List<PlaceAmenity>();
}
