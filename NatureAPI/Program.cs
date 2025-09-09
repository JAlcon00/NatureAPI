//Author: Jesus Almanza
// Date: 2025-09-09

using NatureAPI.Extensions;
using NatureAPI.Data;
using NatureAPI.Data.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddMappingServices();
builder.Services.AddValidationServices();
builder.Services.AddSwaggerServices();
builder.Services.AddCorsServices();

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configurar pipeline de middleware - IMPORTANTE: Swagger debe estar habilitado en todos los entornos
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nature API v1");
    c.RoutePrefix = string.Empty; // Swagger UI en la raíz (http://localhost:5000)
    c.DocumentTitle = "Nature API - Lugares Naturales de México";
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Aplicar migraciones y seed de datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NatureDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Aplicando migraciones de base de datos...");
        await context.Database.EnsureCreatedAsync();
        
        logger.LogInformation("Aplicando seed de datos...");
        await DataSeeder.SeedAsync(context);
        
        logger.LogInformation("Base de datos inicializada correctamente");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar la base de datos");
        throw;
    }
}

app.Run();
