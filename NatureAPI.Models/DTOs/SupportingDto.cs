namespace NatureAPI.Models.DTOs;

/// <summary>
/// DTO para mostrar información de un sendero
/// </summary>
public class TrailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double DistanceKm { get; set; }
    public int EstimatedTimeMinutes { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool IsLoop { get; set; }
}

/// <summary>
/// DTO para mostrar información de una fotografía
/// </summary>
public class PhotoDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// DTO para mostrar información de una reseña
/// </summary>
public class ReviewDto
{
    public int Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO para mostrar información de una amenidad
/// </summary>
public class AmenityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
