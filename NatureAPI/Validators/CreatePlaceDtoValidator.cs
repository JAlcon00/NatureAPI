using FluentValidation;
using NatureAPI.Models.DTOs;

namespace NatureAPI.Validators;

/// <summary>
/// Validador para la creación de nuevos lugares naturales
/// </summary>
public class CreatePlaceDtoValidator : AbstractValidator<CreatePlaceDto>
{
    public CreatePlaceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del lugar es obligatorio")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria")
            .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres")
            .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("La categoría es obligatoria")
            .Must(BeValidCategory).WithMessage("La categoría debe ser una de las siguientes: Parque Nacional, Cascada, Mirador, Cenote, Volcán, Sendero");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(14.5, 32.7).WithMessage("La latitud debe estar dentro del territorio mexicano (14.5° a 32.7°)");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-118.4, -86.7).WithMessage("La longitud debe estar dentro del territorio mexicano (-118.4° a -86.7°)");

        RuleFor(x => x.ElevationMeters)
            .GreaterThanOrEqualTo(-500).WithMessage("La elevación no puede ser menor a -500 metros")
            .LessThanOrEqualTo(6000).WithMessage("La elevación no puede ser mayor a 6000 metros");

        RuleFor(x => x.EntryFee)
            .GreaterThanOrEqualTo(0).WithMessage("El costo de entrada no puede ser negativo")
            .LessThanOrEqualTo(10000).WithMessage("El costo de entrada parece excesivo (máximo $10,000 MXN)");

        RuleFor(x => x.OpeningHours)
            .MaximumLength(100).WithMessage("Los horarios no pueden exceder 100 caracteres");
    }

    /// <summary>
    /// Valida que la categoría sea una de las permitidas
    /// </summary>
    private static bool BeValidCategory(string category)
    {
        var validCategories = new[]
        {
            "Parque Nacional", "Cascada", "Mirador", "Cenote", 
            "Volcán", "Sendero", "Playa", "Laguna", "Cueva", "Desierto"
        };

        return validCategories.Contains(category, StringComparer.OrdinalIgnoreCase);
    }
}
