using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureAPI.Models.Entities;

/// <summary>
/// Entidad que representa una fotografía asociada a un lugar natural
/// </summary>
[Table("Photos")]
public class Photo
{
    /// <summary>
    /// Identificador único de la fotografía
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identificador del lugar al que pertenece la fotografía
    /// </summary>
    [Required]
    [ForeignKey(nameof(Place))]
    public int PlaceId { get; set; }

    /// <summary>
    /// URL de la imagen
    /// </summary>
    [Required]
    [MaxLength(500)]
    [Url(ErrorMessage = "La URL debe tener un formato válido")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Descripción opcional de la imagen
    /// </summary>
    [MaxLength(300)]
    public string? Description { get; set; }

    // Propiedades de navegación
    /// <summary>
    /// Lugar al que pertenece la fotografía
    /// </summary>
    public virtual Place Place { get; set; } = null!;
}
