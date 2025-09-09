using Microsoft.EntityFrameworkCore;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data;

/// <summary>
/// Contexto de base de datos para la API de lugares naturales de México
/// </summary>
public class NatureDbContext : DbContext
{
    public NatureDbContext(DbContextOptions<NatureDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Conjunto de entidades Place (lugares naturales)
    /// </summary>
    public DbSet<Place> Places { get; set; }

    /// <summary>
    /// Conjunto de entidades Trail (senderos)
    /// </summary>
    public DbSet<Trail> Trails { get; set; }

    /// <summary>
    /// Conjunto de entidades Photo (fotografías)
    /// </summary>
    public DbSet<Photo> Photos { get; set; }

    /// <summary>
    /// Conjunto de entidades Review (reseñas)
    /// </summary>
    public DbSet<Review> Reviews { get; set; }

    /// <summary>
    /// Conjunto de entidades Amenity (amenidades)
    /// </summary>
    public DbSet<Amenity> Amenities { get; set; }

    /// <summary>
    /// Conjunto de entidades PlaceAmenity (tabla puente)
    /// </summary>
    public DbSet<PlaceAmenity> PlaceAmenities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones desde archivos separados
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NatureDbContext).Assembly);
    }
}
