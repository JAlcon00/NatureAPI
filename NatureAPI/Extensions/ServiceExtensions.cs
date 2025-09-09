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
        services.AddDbContext<NatureDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
}
