namespace NatureAPI.Models.DTOs;

/// <summary>
/// DTO para mostrar información básica de un lugar natural
/// </summary>
public class PlaceListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int ElevationMeters { get; set; }
    public bool Accessible { get; set; }
    public double EntryFee { get; set; }
    public string OpeningHours { get; set; } = string.Empty;
}

/// <summary>
/// DTO para mostrar información detallada de un lugar natural
/// </summary>
public class PlaceDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int ElevationMeters { get; set; }
    public bool Accessible { get; set; }
    public double EntryFee { get; set; }
    public string OpeningHours { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<TrailDto> Trails { get; set; } = new();
    public List<PhotoDto> Photos { get; set; } = new();
    public List<ReviewDto> Reviews { get; set; } = new();
    public List<AmenityDto> Amenities { get; set; } = new();
}

/// <summary>
/// DTO para crear un nuevo lugar natural
/// </summary>
public class CreatePlaceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int ElevationMeters { get; set; }
    public bool Accessible { get; set; }
    public double EntryFee { get; set; }
    public string OpeningHours { get; set; } = string.Empty;
}
