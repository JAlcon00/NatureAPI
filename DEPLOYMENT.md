# 🚀 Guía de Despliegue CI/CD - NatureAPI

## 📋 Pre-requisitos Completados

✅ **Integración OpenAI** - Servicio `AiSummaryService` configurado  
✅ **Docker** - Dockerfile optimizado con multi-stage build y health checks  
✅ **GitHub Actions** - Pipeline CI/CD completo  
✅ **Configuración** - Variables de entorno preparadas  

---

## 🔧 Configuración Inicial

### 1. Secretos de GitHub (Settings → Secrets and variables → Actions)

Debes agregar los siguientes **Repository Secrets**:

#### Obligatorios:
```
OPENAI_API_KEY          # Tu API key de OpenAI (https://platform.openai.com/api-keys)
DOCKER_USERNAME         # Tu usuario de Docker Hub
DOCKER_PASSWORD         # Tu token de Docker Hub o contraseña
```

#### Opcionales (según plataforma de deploy):

**Para Railway:**
```
RAILWAY_TOKEN           # Token de Railway CLI
RAILWAY_PROJECT_ID      # ID del proyecto en Railway
```

**Para Render:**
```
RENDER_DEPLOY_HOOK_URL  # URL del deploy hook de Render
```

**Para Azure:**
```
AZURE_WEBAPP_NAME              # Nombre de tu Web App
AZURE_WEBAPP_PUBLISH_PROFILE   # Perfil de publicación XML
```

**Para Fly.io:**
```
FLY_API_TOKEN           # Token de autenticación de Fly.io
```

#### Post-Deploy:
```
API_URL                 # URL pública de tu API (ej: https://nature-api.railway.app)
```

---

### 2. Variables de Repositorio (Repository Variables)

```
DEPLOY_PLATFORM         # Valor: railway | render | azure | flyio
```

---

## 🐳 Opción A: Despliegue en Railway (Recomendado)

### ¿Por qué Railway?
- ✅ Free tier generoso (500 horas/mes)
- ✅ Soporte nativo para Docker
- ✅ Base de datos PostgreSQL incluida
- ✅ Despliegue automático desde GitHub
- ✅ Variables de entorno fáciles de configurar

### Pasos:

1. **Crear cuenta en Railway**: https://railway.app/

2. **Instalar Railway CLI** (para CI/CD):
   ```bash
   npm install -g @railway/cli
   railway login
   ```

3. **Crear nuevo proyecto**:
   ```bash
   railway init
   railway link
   ```

4. **Agregar base de datos PostgreSQL**:
   - En Railway dashboard: "New" → "Database" → "PostgreSQL"
   - Copia la connection string generada

5. **Convertir el proyecto a PostgreSQL** (Railway no soporta SQL Server gratis):
   
   Actualizar `NatureAPI.csproj`:
   ```xml
   <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.6" />
   ```
   
   Actualizar `ServiceExtensions.cs`:
   ```csharp
   services.AddDbContext<NatureDbContext>(options =>
       options.UseNpgsql(connectionString));
   ```

6. **Configurar variables de entorno en Railway**:
   ```
   DATABASE_URL            # Auto-configurado por Railway
   OPENAI_API_KEY          # Tu API key de OpenAI
   AI__OpenAI__Model       # gpt-4o-mini
   ```

7. **Desplegar**:
   ```bash
   railway up
   ```

8. **Configurar GitHub Actions**:
   - Agrega `RAILWAY_TOKEN` y `RAILWAY_PROJECT_ID` a GitHub Secrets
   - Set `DEPLOY_PLATFORM=railway` en Variables

---

## 🎨 Opción B: Despliegue en Render

### Pasos:

1. **Crear cuenta**: https://render.com/

2. **New Web Service** → "Deploy an existing image from a registry"

3. **Conectar con Docker Hub**:
   - Image URL: `tu-usuario/nature-api:latest`

4. **Configurar**:
   - Plan: Free
   - Region: Oregon (US West)
   - Health Check Path: `/health`

5. **Variables de entorno**:
   ```
   DATABASE_URL=postgresql://...  # Usa Render PostgreSQL
   OPENAI_API_KEY=sk-...
   AI__OpenAI__Model=gpt-4o-mini
   ```

6. **Deploy Hook**:
   - Copia el "Deploy Hook URL" de Render
   - Agrégalo como `RENDER_DEPLOY_HOOK_URL` en GitHub Secrets
   - Set `DEPLOY_PLATFORM=render`

---

## ☁️ Opción C: Despliegue en Azure

### Pasos:

1. **Crear recursos**:
   ```bash
   az group create --name NatureAPI-RG --location eastus
   az appservice plan create --name NatureAPI-Plan --resource-group NatureAPI-RG --sku B1 --is-linux
   az webapp create --resource-group NatureAPI-RG --plan NatureAPI-Plan --name nature-api-<tu-nombre> --deployment-container-image-name ghcr.io/tu-usuario/nature-api:latest
   ```

2. **Azure SQL Database**:
   ```bash
   az sql server create --name nature-api-sql --resource-group NatureAPI-RG --location eastus --admin-user sqladmin --admin-password <password>
   az sql db create --resource-group NatureAPI-RG --server nature-api-sql --name NatureDB --service-objective S0
   ```

3. **Configurar variables**:
   ```bash
   az webapp config appsettings set --resource-group NatureAPI-RG --name nature-api-<tu-nombre> --settings \
     "ConnectionStrings__DefaultConnection=Server=..." \
     "AI__OpenAI__ApiKey=sk-..." \
     "AI__OpenAI__Model=gpt-4o-mini"
   ```

4. **Descargar perfil de publicación** y agregarlo como `AZURE_WEBAPP_PUBLISH_PROFILE`

5. **Set** `DEPLOY_PLATFORM=azure`

---

## 🪂 Opción D: Despliegue en Fly.io

### Pasos:

1. **Instalar flyctl**:
   ```bash
   brew install flyctl  # macOS
   flyctl auth signup   # o login
   ```

2. **Crear app**:
   ```bash
   flyctl launch --no-deploy
   ```

3. **Configurar `fly.toml`** (ya generado):
   ```toml
   app = "nature-api"
   
   [env]
     ASPNETCORE_URLS = "http://0.0.0.0:8080"
   
   [[services]]
     internal_port = 8080
     protocol = "tcp"
   
   [health_checks]
     [health_checks.alive]
       path = "/health"
   ```

4. **Secretos**:
   ```bash
   flyctl secrets set OPENAI_API_KEY=sk-...
   flyctl secrets set AI__OpenAI__Model=gpt-4o-mini
   ```

5. **PostgreSQL**:
   ```bash
   flyctl postgres create
   flyctl postgres attach <postgres-app-name>
   ```

6. **Deploy**:
   ```bash
   flyctl deploy
   ```

7. **Set** `FLY_API_TOKEN` en GitHub Secrets y `DEPLOY_PLATFORM=flyio`

---

## 🧪 Probar el Despliegue

### 1. Health Check
```bash
curl https://tu-api.railway.app/health
```

### 2. Swagger UI
Visita: `https://tu-api.railway.app/`

### 3. Endpoint con IA
```bash
curl https://tu-api.railway.app/api/places/1/summary
```

Respuesta esperada:
```json
{
  "placeId": 1,
  "summary": "Cascadas de Agua Azul: impresionante cascada en Chiapas..."
}
```

---

## 📊 Monitoreo del Pipeline

1. **GitHub Actions**: Ve a la pestaña "Actions" en tu repositorio
2. **Logs en tiempo real**: Click en el workflow running
3. **Notificaciones**: Configura en Settings → Notifications

---

## 🎥 Checklist para el Video Demo

- [ ] Mostrar código del `AiSummaryService.cs`
- [ ] Mostrar `Dockerfile` optimizado
- [ ] Mostrar pipeline `.github/workflows/ci-cd.yml`
- [ ] Push a GitHub y ver Actions ejecutándose
- [ ] Mostrar logs de build y deploy
- [ ] Abrir Swagger UI en producción
- [ ] Llamar a `/api/places/1/summary` y ver respuesta con IA
- [ ] Mostrar health check funcionando
- [ ] Verificar imagen en Docker Hub/GHCR

---

## 🐛 Troubleshooting

### Error: "OpenAI API Key not configured"
- Verifica que `OPENAI_API_KEY` esté en variables de entorno
- Formato: `AI__OpenAI__ApiKey` (doble underscore)

### Error: "Database connection failed"
- Verifica connection string
- Railway/Render: Usa PostgreSQL, no SQL Server
- Azure: Asegúrate que el firewall permita Azure Services

### Error: Docker build fails
- Revisa que todos los `.csproj` sean copiados correctamente
- Verifica que no falten dependencias en `NatureAPI.csproj`

### Error: Health check failing
- Asegúrate que `/health` endpoint exista (HealthController)
- Verifica que el puerto 8080 esté expuesto

---

## 📚 Recursos Adicionales

- [Railway Docs](https://docs.railway.app/)
- [Render Docs](https://render.com/docs)
- [Azure App Service](https://docs.microsoft.com/azure/app-service/)
- [Fly.io Docs](https://fly.io/docs/)
- [OpenAI API Reference](https://platform.openai.com/docs/api-reference)

---

## ✅ Checklist Final

- [ ] Integración OpenAI funcionando localmente
- [ ] Docker image builds correctamente
- [ ] Pipeline CI/CD configurado en GitHub
- [ ] Secretos agregados a GitHub
- [ ] Desplegado en plataforma cloud elegida
- [ ] URL pública funcionando
- [ ] Endpoint `/api/places/1/summary` responde con IA
- [ ] Video demo grabado (2-5 min)
- [ ] README actualizado con URLs públicas

---

**¡Listo para el examen! 🎓**

