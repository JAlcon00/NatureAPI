using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Place
/// </summary>
public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        // Configuración de la tabla
        builder.ToTable("Places");

        // Clave primaria
        builder.HasKey(p => p.Id);

        // Configuración de propiedades
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Latitude)
            .IsRequired()
            .HasPrecision(18, 6);

        builder.Property(p => p.Longitude)
            .IsRequired()
            .HasPrecision(18, 6);

        builder.Property(p => p.EntryFee)
            .HasPrecision(18, 2);

        builder.Property(p => p.OpeningHours)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Relaciones
        builder.HasMany(p => p.Trails)
            .WithOne(t => t.Place)
            .HasForeignKey(t => t.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Photos)
            .WithOne(ph => ph.Place)
            .HasForeignKey(ph => ph.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Reviews)
            .WithOne(r => r.Place)
            .HasForeignKey(r => r.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices para mejorar rendimiento
        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => new { p.Latitude, p.Longitude });
        builder.HasIndex(p => p.CreatedAt);
    }
}
