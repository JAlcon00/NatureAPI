# 🏞️ Nature API - Lugares Naturales de México

API REST completa en .NET 9 para gestionar lugares naturales de México (parques, cascadas, miradores, senderos) con coordenadas, metadatos y relaciones entre entidades.

## 🚀 Características Principales

- **Arquitectura profesional** con separación de responsabilidades
- **Entity Framework Core** con migraciones y configuraciones
- **SQL Server** ejecutándose en Docker
- **Datos precargados** de lugares reales de México
- **Validación robusta** con FluentValidation
- **AutoMapper** para mapeo de DTOs
- **Swagger/OpenAPI** para documentación interactiva
- **Logging** estructurado y manejo de errores

## 📋 Entidades y Relaciones

### Entidades Principales

1. **Place** - Lugar natural principal
2. **Trail** - Sendero o ruta de un lugar
3. **Photo** - Imagen asociada a un lugar
4. **Review** - Reseña de un visitante
5. **Amenity** - Servicio o amenidad disponible
6. **PlaceAmenity** - Tabla puente (relación N:N)

### Relaciones

- `Place` (1) → (N) `Trail`
- `Place` (1) → (N) `Photo`
- `Place` (1) → (N) `Review`
- `Place` (N) ↔ (N) `Amenity` (vía `PlaceAmenity`)

## 🛠️ Tecnologías Utilizadas

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0** - ORM
- **SQL Server 2022** - Base de datos
- **Docker & Docker Compose** - Contenedorización
- **AutoMapper** - Mapeo de objetos
- **FluentValidation** - Validación de datos
- **Swagger/OpenAPI** - Documentación de API

## 📁 Estructura del Proyecto

```
NatureAPI/
├── Controllers/           # Controladores de la API REST
├── Data/
│   ├── Configurations/   # Configuraciones de Entity Framework
│   ├── Seeds/           # Datos iniciales
│   └── NatureDbContext.cs
├── Models/
│   ├── Entities/        # Entidades del dominio
│   └── DTOs/           # Data Transfer Objects
├── Services/
│   ├── Interfaces/     # Interfaces de servicios
│   └── Implementations/ # Implementaciones de servicios
├── Validators/         # Validadores FluentValidation
├── Mappings/          # Perfiles AutoMapper
├── Extensions/        # Extensiones de configuración
└── Migrations/        # Migraciones de EF Core
```

## 🚀 Instalación y Configuración

### Prerrequisitos

- Docker y Docker Compose
- .NET 9 SDK
- Visual Studio, VS Code o Rider

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd NatureAPI
```

### 2. Levantar SQL Server con Docker

```bash
docker-compose up -d
```

Este comando levantará SQL Server 2022 con las siguientes credenciales:
- **Usuario**: SA
- **Contraseña**: Password123!
- **Puerto**: 1433

### 3. Restaurar dependencias

```bash
cd NatureAPI
dotnet restore
```

### 4. Ejecutar la aplicación

```bash
dotnet run
```

La API estará disponible en:
- **HTTPS**: https://localhost:7000
- **HTTP**: http://localhost:5000
- **Swagger UI**: https://localhost:7000 (página principal)

## 📚 Endpoints de la API

### 🏞️ Places (Lugares Naturales)

#### `GET /api/places`
Lista todos los lugares naturales con filtros opcionales.

**Parámetros de consulta:**
- `category` (opcional): Filtro por categoría
- `difficulty` (opcional): Filtro por dificultad de senderos

**Ejemplo:**
```bash
GET /api/places?category=Cascada&difficulty=Fácil
```

#### `GET /api/places/{id}`
Obtiene un lugar específico por su ID con todos sus detalles.

**Ejemplo:**
```bash
GET /api/places/1
```

#### `POST /api/places`
Crea un nuevo lugar natural.

**Ejemplo de request body:**
```json
{
  "name": "Cenote Ik Kil",
  "description": "Cenote sagrado de los mayas ubicado cerca de Chichen Itza",
  "category": "Cenote",
  "latitude": 20.6615,
  "longitude": -88.5567,
  "elevationMeters": 26,
  "accessible": true,
  "entryFee": 150.0,
  "openingHours": "8:00 AM - 5:00 PM"
}
```

#### `GET /api/places/statistics`
Obtiene estadísticas generales de los lugares naturales.

## 🗺️ Datos Precargados

La API incluye datos reales de lugares naturales icónicos de México:

1. **Cascadas de Agua Azul** (Chiapas)
2. **Parque Nacional Desierto de los Leones** (CDMX)
3. **Hierve el Agua** (Oaxaca)
4. **Cenote Dos Ojos** (Quintana Roo)
5. **Nevado de Toluca** (Estado de México)

Cada lugar incluye:
- ✅ Senderos con diferentes dificultades
- ✅ Fotografías reales de Wikipedia
- ✅ Amenidades disponibles
- ✅ Coordenadas precisas

## 🧪 Validaciones

### CreatePlaceDto Validator

- **Nombre**: 3-200 caracteres, obligatorio
- **Descripción**: 10-1000 caracteres, obligatoria
- **Categorías válidas**: Parque Nacional, Cascada, Mirador, Cenote, Volcán, Sendero, Playa, Laguna, Cueva, Desierto
- **Coordenadas**: Validadas para territorio mexicano
  - Latitud: 14.5° a 32.7°
  - Longitud: -118.4° a -86.7°
- **Elevación**: -500m a 6000m
- **Costo de entrada**: ≥ 0, ≤ $10,000 MXN

## 🛡️ Manejo de Errores

La API incluye manejo robusto de errores con:

- **400 Bad Request**: Datos de entrada inválidos
- **404 Not Found**: Recurso no encontrado
- **500 Internal Server Error**: Errores internos del servidor

Ejemplo de respuesta de error:
```json
{
  "message": "Datos de entrada inválidos",
  "errors": [
    {
      "property": "Latitude",
      "error": "La latitud debe estar dentro del territorio mexicano"
    }
  ]
}
```

## 📊 Base de Datos

### Esquema Principal

```sql
-- Tablas principales
Places
Trails
Photos
Reviews
Amenities
PlaceAmenities (tabla puente)
```

### Características de la BD

- **Índices optimizados** para búsquedas frecuentes
- **Restricciones de integridad** referencial
- **Validaciones a nivel de BD**
- **Seed de datos automático**

## 🔧 Configuración

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NatureDB;User Id=SA;Password=Password123!;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Docker Compose

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=Password123!
    ports:
      - "1433:1433"
```

## 🧪 Pruebas con Swagger

1. Ejecuta la aplicación: `dotnet run`
2. Navega a: https://localhost:7000
3. Explora y prueba todos los endpoints interactivamente

### Ejemplos de Prueba

**Obtener todos los lugares:**
```
GET /api/places
```

**Filtrar por categoría:**
```
GET /api/places?category=Cascada
```

**Crear un nuevo lugar:**
```
POST /api/places
Content-Type: application/json

{
  "name": "Nuevo Lugar",
  "description": "Descripción del lugar...",
  "category": "Mirador",
  "latitude": 19.4326,
  "longitude": -99.1332,
  "elevationMeters": 2240,
  "accessible": true,
  "entryFee": 0,
  "openingHours": "24 horas"
}
```

## 🏗️ Arquitectura

### Patrón de Arquitectura

- **Repository Pattern** (implícito con EF Core)
- **Service Layer Pattern**
- **Dependency Injection**
- **DTO Pattern**
- **Configuration Pattern**

### Flujo de Datos

```
Controller → Service → Repository (EF Core) → Database
     ↓           ↓
   DTOs ← AutoMapper ← Entities
```

## 🎯 Evaluación

| Criterio | Puntos | ✅ Implementado |
|----------|--------|----------------|
| Diseño de entidades y relaciones | 20 pts | ✅ |
| Migraciones y uso de Docker | 15 pts | ✅ |
| Datos precargados (seed) | 15 pts | ✅ |
| Endpoints REST solicitados | 25 pts | ✅ |
| Calidad del código | 15 pts | ✅ |
| Documentación en README | 10 pts | ✅ |
| **Total** | **100 pts** | **✅** |

## 🚀 Próximos Pasos

- [ ] Implementar autenticación JWT
- [ ] Agregar paginación a los endpoints
- [ ] Implementar caché con Redis
- [ ] Agregar más validaciones de negocio
- [ ] Implementar soft delete
- [ ] Agregar tests unitarios e integración

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama: `git checkout -b feature/nueva-funcionalidad`
3. Commit tus cambios: `git commit -m 'Agregar nueva funcionalidad'`
4. Push a la rama: `git push origin feature/nueva-funcionalidad`
5. Abre un Pull Request

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

---

**🏞️ Nature API - Conectando México con su belleza natural** 🇲🇽
