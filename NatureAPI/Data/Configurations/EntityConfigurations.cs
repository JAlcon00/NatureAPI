using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Trail
/// </summary>
public class TrailConfiguration : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("Trails");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.DistanceKm)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.Property(t => t.Difficulty)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.Path)
            .HasMaxLength(2000);

        // Índices
        builder.HasIndex(t => t.PlaceId);
        builder.HasIndex(t => t.Difficulty);
    }
}

/// <summary>
/// Configuración de Entity Framework para la entidad Photo
/// </summary>
public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("Photos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(300);

        builder.HasIndex(p => p.PlaceId);
    }
}

/// <summary>
/// Configuración de Entity Framework para la entidad Review
/// </summary>
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Author)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(r => r.PlaceId);
        builder.HasIndex(r => r.Rating);
        builder.HasIndex(r => r.CreatedAt);
    }
}
