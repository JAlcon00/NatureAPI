# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias (layer caching)
COPY ["NatureAPI/NatureAPI.csproj", "NatureAPI/"]
COPY ["NatureAPI.Models/NatureAPI.Models.csproj", "NatureAPI.Models/"]
RUN dotnet restore "NatureAPI/NatureAPI.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar y publicar en modo Release
WORKDIR "/src/NatureAPI"
RUN dotnet publish "NatureAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Variables de entorno para producción
ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copiar archivos compilados
COPY --from=build /app/publish .

# Exponer puerto
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl --fail http://localhost:8080/health || exit 1

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "NatureAPI.dll"]
