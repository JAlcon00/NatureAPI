using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Amenity
/// </summary>
public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable("Amenities");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Índice único para evitar amenidades duplicadas
        builder.HasIndex(a => a.Name)
            .IsUnique();
    }
}

/// <summary>
/// Configuración de Entity Framework para la tabla puente PlaceAmenity
/// </summary>
public class PlaceAmenityConfiguration : IEntityTypeConfiguration<PlaceAmenity>
{
    public void Configure(EntityTypeBuilder<PlaceAmenity> builder)
    {
        builder.ToTable("PlaceAmenities");

        // Clave primaria compuesta
        builder.HasKey(pa => new { pa.PlaceId, pa.AmenityId });

        // Relación con Place
        builder.HasOne(pa => pa.Place)
            .WithMany(p => p.PlaceAmenities)
            .HasForeignKey(pa => pa.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con Amenity
        builder.HasOne(pa => pa.Amenity)
            .WithMany(a => a.PlaceAmenities)
            .HasForeignKey(pa => pa.AmenityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices para mejorar rendimiento
        builder.HasIndex(pa => pa.PlaceId);
        builder.HasIndex(pa => pa.AmenityId);
    }
}
