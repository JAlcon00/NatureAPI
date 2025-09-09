using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureAPI.Models.Entities;

/// <summary>
/// Entidad que representa una reseña de un visitante sobre un lugar natural
/// </summary>
[Table("Reviews")]
public class Review
{
    /// <summary>
    /// Identificador único de la reseña
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identificador del lugar reseñado
    /// </summary>
    [Required]
    [ForeignKey(nameof(Place))]
    public int PlaceId { get; set; }

    /// <summary>
    /// Nombre del autor de la reseña
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Calificación del lugar (1-5 estrellas)
    /// </summary>
    [Required]
    [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5")]
    public int Rating { get; set; }

    /// <summary>
    /// Comentario detallado del visitante
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora de creación de la reseña
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Propiedades de navegación
    /// <summary>
    /// Lugar reseñado
    /// </summary>
    public virtual Place Place { get; set; } = null!;
}
