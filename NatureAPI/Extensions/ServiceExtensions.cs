using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Services.Interfaces;
using NatureAPI.Services.Implementations;
using FluentValidation;
using NatureAPI.Validators;
using NatureAPI.Models.DTOs;
using NatureAPI.Mappings;

namespace NatureAPI.Extensions;

/// <summary>
/// Extensiones para configurar los servicios de la aplicación
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configura los servicios de base de datos
    /// </summary>
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Detectar si estamos en Railway (tiene DATABASE_URL)
        var databaseUrl = configuration["DATABASE_URL"] ?? Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrEmpty(databaseUrl))
        {
            var logger = services.BuildServiceProvider().GetService<ILogger<object>>();
            
            try
            {
                // Parsear DATABASE_URL: postgres://user:password@host:port/database
                var uri = new Uri(databaseUrl.Replace("postgres://", "postgresql://"));
                
                connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]};SSL Mode=Require;Trust Server Certificate=true";
                
                logger?.LogInformation("DATABASE_URL parseada correctamente para Npgsql");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error al parsear DATABASE_URL, usando DefaultConnection");
            }
        }
        else
        {
            // Fallback: usar connection string de configuración
            var logger = services.BuildServiceProvider().GetService<ILogger<object>>();
            logger?.LogWarning("DATABASE_URL no encontrada, usando DefaultConnection de appsettings");
        }
        
        services.AddDbContext<NatureDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions
                    .EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null)
            ));

        return services;
    }

    /// <summary>
    /// Configura los servicios de negocio
    /// </summary>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IPlaceService, PlaceService>();
        
        return services;
    }

    /// <summary>
    /// Configura AutoMapper
    /// </summary>
    public static IServiceCollection AddMappingServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        
        return services;
    }

    /// <summary>
    /// Configura FluentValidation
    /// </summary>
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreatePlaceDto>, CreatePlaceDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreatePlaceDtoValidator>();
        
        return services;
    }

    /// <summary>
    /// Configura Swagger/OpenAPI
    /// </summary>
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Nature API - Lugares Naturales de México",
                Version = "v1",
                Description = "API REST para gestionar lugares naturales de México (parques, cascadas, miradores, senderos) con coordenadas y metadatos.",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Equipo de Desarrollo",
                    Email = "desarrollo@natureapi.mx"
                }
            });

            // Incluir comentarios XML
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    /// <summary>
    /// Configura CORS
    /// </summary>
    public static IServiceCollection AddCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    /// <summary>
    /// Configura HttpClient y servicios de IA
    /// </summary>
    public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IAiSummaryService, AiSummaryService>();

        return services;
    }
}
