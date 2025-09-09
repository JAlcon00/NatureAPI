using Microsoft.EntityFrameworkCore;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data.Seeds;

/// <summary>
/// Clase para cargar datos iniciales en la base de datos
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Aplica todas las semillas de datos a la base de datos
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public static async Task SeedAsync(NatureDbContext context)
    {
        // Asegurar que la base de datos esté creada
        await context.Database.EnsureCreatedAsync();

        // Verificar si ya hay datos
        if (await context.Places.AnyAsync())
        {
            return; // Ya hay datos, no es necesario hacer seed
        }

        // Seed de amenidades
        await SeedAmenitiesAsync(context);

        // Seed de lugares naturales
        await SeedPlacesAsync(context);

        // Seed de senderos
        await SeedTrailsAsync(context);

        // Seed de fotografías
        await SeedPhotosAsync(context);

        // Seed de relaciones Place-Amenity
        await SeedPlaceAmenitiesAsync(context);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Carga datos iniciales de amenidades
    /// </summary>
    private static async Task SeedAmenitiesAsync(NatureDbContext context)
    {
        var amenities = new List<Amenity>
        {
            new() { Id = 1, Name = "Baños" },
            new() { Id = 2, Name = "Estacionamiento" },
            new() { Id = 3, Name = "Mirador" },
            new() { Id = 4, Name = "Área de picnic" },
            new() { Id = 5, Name = "Senderos marcados" },
            new() { Id = 6, Name = "Guías turísticos" },
            new() { Id = 7, Name = "Tienda de souvenirs" },
            new() { Id = 8, Name = "Restaurante" },
            new() { Id = 9, Name = "Acceso para discapacitados" },
            new() { Id = 10, Name = "Camping" },
            new() { Id = 11, Name = "Puente colgante" },
            new() { Id = 12, Name = "Zona de natación" }
        };

        context.Amenities.AddRange(amenities);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Carga datos iniciales de lugares naturales de México
    /// </summary>
    private static async Task SeedPlacesAsync(NatureDbContext context)
    {
        var places = new List<Place>
        {
            new()
            {
                Id = 1,
                Name = "Cascadas de Agua Azul",
                Description = "Impresionantes cascadas de agua turquesa ubicadas en Chiapas, rodeadas de exuberante vegetación tropical. Un paraíso natural perfecto para la relajación y el ecoturismo.",
                Category = "Cascada",
                Latitude = 17.2583,
                Longitude = -92.1167,
                ElevationMeters = 180,
                Accessible = true,
                EntryFee = 45.0,
                OpeningHours = "8:00 AM - 5:00 PM",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 2,
                Name = "Parque Nacional Desierto de los Leones",
                Description = "Primer parque nacional de México, ubicado en la Ciudad de México. Ideal para senderismo, ciclismo de montaña y disfrutar de la naturaleza cerca de la urbe.",
                Category = "Parque Nacional",
                Latitude = 19.3069,
                Longitude = -99.3128,
                ElevationMeters = 3200,
                Accessible = false,
                EntryFee = 0.0,
                OpeningHours = "6:00 AM - 6:00 PM",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 3,
                Name = "Hierve el Agua",
                Description = "Formaciones rocosas de carbonato de calcio que semejan cascadas petrificadas en Oaxaca. Un lugar único para nadar en piscinas naturales con vistas espectaculares.",
                Category = "Mirador",
                Latitude = 16.8667,
                Longitude = -96.2833,
                ElevationMeters = 1800,
                Accessible = false,
                EntryFee = 35.0,
                OpeningHours = "8:00 AM - 6:00 PM",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 4,
                Name = "Cenote Dos Ojos",
                Description = "Sistema de cavernas subacuáticas en la Riviera Maya, perfecto para snorkel y buceo. Aguas cristalinas que conectan con el segundo sistema de cuevas más largo del mundo.",
                Category = "Cenote",
                Latitude = 20.2239,
                Longitude = -87.3539,
                ElevationMeters = 5,
                Accessible = true,
                EntryFee = 350.0,
                OpeningHours = "8:00 AM - 5:00 PM",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 5,
                Name = "Nevado de Toluca",
                Description = "Volcán extinto en el Estado de México con dos lagunas cratéricas: Laguna del Sol y Laguna de la Luna. Ideal para montañismo y contemplar paisajes únicos de alta montaña.",
                Category = "Volcán",
                Latitude = 19.1089,
                Longitude = -99.7581,
                ElevationMeters = 4680,
                Accessible = false,
                EntryFee = 60.0,
                OpeningHours = "6:00 AM - 4:00 PM",
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Places.AddRange(places);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Carga datos iniciales de senderos para los lugares naturales
    /// </summary>
    private static async Task SeedTrailsAsync(NatureDbContext context)
    {
        var trails = new List<Trail>
        {
            // Senderos para Cascadas de Agua Azul
            new()
            {
                Id = 1,
                PlaceId = 1,
                Name = "Sendero Principal a las Cascadas",
                DistanceKm = 1.2,
                EstimatedTimeMinutes = 45,
                Difficulty = "Fácil",
                Path = "19.1089,-99.7581;19.1095,-99.7575;19.1102,-99.7569",
                IsLoop = false
            },
            new()
            {
                Id = 2,
                PlaceId = 1,
                Name = "Circuito Completo de Cascadas",
                DistanceKm = 3.5,
                EstimatedTimeMinutes = 120,
                Difficulty = "Moderado",
                Path = "19.1089,-99.7581;19.1095,-99.7575;19.1102,-99.7569;19.1089,-99.7581",
                IsLoop = true
            },

            // Senderos para Parque Nacional Desierto de los Leones
            new()
            {
                Id = 3,
                PlaceId = 2,
                Name = "Sendero del Convento",
                DistanceKm = 2.8,
                EstimatedTimeMinutes = 90,
                Difficulty = "Moderado",
                Path = "19.3069,-99.3128;19.3075,-99.3135;19.3082,-99.3142",
                IsLoop = false
            },
            new()
            {
                Id = 4,
                PlaceId = 2,
                Name = "Circuito de los Venados",
                DistanceKm = 6.2,
                EstimatedTimeMinutes = 180,
                Difficulty = "Difícil",
                Path = "19.3069,-99.3128;19.3075,-99.3135;19.3082,-99.3142;19.3069,-99.3128",
                IsLoop = true
            },

            // Senderos para Hierve el Agua
            new()
            {
                Id = 5,
                PlaceId = 3,
                Name = "Sendero a las Piscinas Naturales",
                DistanceKm = 0.8,
                EstimatedTimeMinutes = 30,
                Difficulty = "Fácil",
                Path = "16.8667,-96.2833;16.8672,-96.2838;16.8677,-96.2843",
                IsLoop = false
            },

            // Senderos para Cenote Dos Ojos
            new()
            {
                Id = 6,
                PlaceId = 4,
                Name = "Sendero de Acceso al Cenote",
                DistanceKm = 0.5,
                EstimatedTimeMinutes = 15,
                Difficulty = "Fácil",
                Path = "20.2239,-87.3539;20.2242,-87.3542;20.2245,-87.3545",
                IsLoop = false
            },

            // Senderos para Nevado de Toluca
            new()
            {
                Id = 7,
                PlaceId = 5,
                Name = "Ascenso a las Lagunas",
                DistanceKm = 4.5,
                EstimatedTimeMinutes = 240,
                Difficulty = "Extremo",
                Path = "19.1089,-99.7581;19.1095,-99.7575;19.1102,-99.7569",
                IsLoop = false
            }
        };

        context.Trails.AddRange(trails);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Carga datos iniciales de fotografías con URLs reales y coherentes
    /// </summary>
    private static async Task SeedPhotosAsync(NatureDbContext context)
    {
        var photos = new List<Photo>
        {
            // Fotos para Cascadas de Agua Azul
            new()
            {
                Id = 1,
                PlaceId = 1,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8b/Agua_Azul_Waterfalls.jpg/1280px-Agua_Azul_Waterfalls.jpg",
                Description = "Vista panorámica de las cascadas de agua turquesa"
            },
            new()
            {
                Id = 2,
                PlaceId = 1,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1d/Cascadas_de_Agua_Azul_2.jpg/1280px-Cascadas_de_Agua_Azul_2.jpg",
                Description = "Piscinas naturales de agua cristalina"
            },

            // Fotos para Parque Nacional Desierto de los Leones
            new()
            {
                Id = 3,
                PlaceId = 2,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9c/Desierto_de_los_Leones.jpg/1280px-Desierto_de_los_Leones.jpg",
                Description = "Bosque de coníferas y senderos naturales"
            },
            new()
            {
                Id = 4,
                PlaceId = 2,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/52/Convento_Desierto_Leones.jpg/1280px-Convento_Desierto_Leones.jpg",
                Description = "Ruinas del antiguo convento carmelita"
            },

            // Fotos para Hierve el Agua
            new()
            {
                Id = 5,
                PlaceId = 3,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Hierve_el_Agua_Oaxaca.jpg/1280px-Hierve_el_Agua_Oaxaca.jpg",
                Description = "Formaciones rocosas que parecen cascadas petrificadas"
            },
            new()
            {
                Id = 6,
                PlaceId = 3,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b8/Hierve_el_Agua_piscinas.jpg/1280px-Hierve_el_Agua_piscinas.jpg",
                Description = "Piscinas naturales con vista al valle de Oaxaca"
            },

            // Fotos para Cenote Dos Ojos
            new()
            {
                Id = 7,
                PlaceId = 4,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Cenote_Dos_Ojos.jpg/1280px-Cenote_Dos_Ojos.jpg",
                Description = "Aguas cristalinas del cenote subterráneo"
            },
            new()
            {
                Id = 8,
                PlaceId = 4,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Cenote_cave_diving.jpg/1280px-Cenote_cave_diving.jpg",
                Description = "Sistema de cavernas subacuáticas para buceo"
            },

            // Fotos para Nevado de Toluca
            new()
            {
                Id = 9,
                PlaceId = 5,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e1/Nevado_de_Toluca_crater.jpg/1280px-Nevado_de_Toluca_crater.jpg",
                Description = "Cráter del volcán con las lagunas del Sol y de la Luna"
            },
            new()
            {
                Id = 10,
                PlaceId = 5,
                Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Laguna_del_Sol_Toluca.jpg/1280px-Laguna_del_Sol_Toluca.jpg",
                Description = "Laguna del Sol en el cráter del Nevado de Toluca"
            }
        };

        context.Photos.AddRange(photos);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Carga relaciones entre lugares y amenidades
    /// </summary>
    private static async Task SeedPlaceAmenitiesAsync(NatureDbContext context)
    {
        var placeAmenities = new List<PlaceAmenity>
        {
            // Cascadas de Agua Azul
            new() { PlaceId = 1, AmenityId = 1 }, // Baños
            new() { PlaceId = 1, AmenityId = 2 }, // Estacionamiento
            new() { PlaceId = 1, AmenityId = 4 }, // Área de picnic
            new() { PlaceId = 1, AmenityId = 6 }, // Guías turísticos
            new() { PlaceId = 1, AmenityId = 7 }, // Tienda de souvenirs
            new() { PlaceId = 1, AmenityId = 8 }, // Restaurante
            new() { PlaceId = 1, AmenityId = 12 }, // Zona de natación

            // Parque Nacional Desierto de los Leones
            new() { PlaceId = 2, AmenityId = 1 }, // Baños
            new() { PlaceId = 2, AmenityId = 2 }, // Estacionamiento
            new() { PlaceId = 2, AmenityId = 4 }, // Área de picnic
            new() { PlaceId = 2, AmenityId = 5 }, // Senderos marcados
            new() { PlaceId = 2, AmenityId = 10 }, // Camping

            // Hierve el Agua
            new() { PlaceId = 3, AmenityId = 1 }, // Baños
            new() { PlaceId = 3, AmenityId = 2 }, // Estacionamiento
            new() { PlaceId = 3, AmenityId = 3 }, // Mirador
            new() { PlaceId = 3, AmenityId = 6 }, // Guías turísticos
            new() { PlaceId = 3, AmenityId = 7 }, // Tienda de souvenirs
            new() { PlaceId = 3, AmenityId = 8 }, // Restaurante
            new() { PlaceId = 3, AmenityId = 12 }, // Zona de natación

            // Cenote Dos Ojos
            new() { PlaceId = 4, AmenityId = 1 }, // Baños
            new() { PlaceId = 4, AmenityId = 2 }, // Estacionamiento
            new() { PlaceId = 4, AmenityId = 6 }, // Guías turísticos
            new() { PlaceId = 4, AmenityId = 7 }, // Tienda de souvenirs
            new() { PlaceId = 4, AmenityId = 12 }, // Zona de natación

            // Nevado de Toluca
            new() { PlaceId = 5, AmenityId = 2 }, // Estacionamiento
            new() { PlaceId = 5, AmenityId = 3 }, // Mirador
            new() { PlaceId = 5, AmenityId = 5 }, // Senderos marcados
            new() { PlaceId = 5, AmenityId = 10 } // Camping
        };

        context.PlaceAmenities.AddRange(placeAmenities);
        await context.SaveChangesAsync();
    }
}
