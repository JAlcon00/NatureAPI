# 🤖 Integración con OpenAI - NatureAPI

## ✅ Estado: COMPLETO

La API ahora incluye integración completa con OpenAI para análisis inteligente de lugares naturales de México.

---

## 🎯 Endpoints con IA

### 1. **Resumen de Lugar Individual** 
```http
GET /api/places/{id}/summary
```

**Descripción**: Genera un resumen atractivo e informativo de un lugar específico usando GPT-4o-mini.

**Ejemplo**:
```bash
curl https://tu-api.com/api/places/1/summary
```

**Respuesta**:
```json
{
  "placeId": 1,
  "summary": "Las Cascadas de Agua Azul en Chiapas son un espectacular conjunto de caídas de agua color turquesa. Con una elevación de 180m, este lugar accesible ofrece senderos de dificultad fácil y amenidades completas. La entrada tiene un costo de $45 MXN. Ideal para visitar de noviembre a abril durante la época seca..."
}
```

**Características**:
- ✅ Incluye información sobre ubicación, elevación, accesibilidad
- ✅ Menciona senderos disponibles y su dificultad
- ✅ Lista amenidades del lugar
- ✅ Consejos de mejor época para visitar
- ✅ Fallback local si OpenAI no está disponible

---

### 2. **Análisis Completo con IA** ⭐ NUEVO
```http
GET /api/places/ai-analyze
```

**Descripción**: Analiza múltiples lugares y genera insights inteligentes, patrones, tendencias y recomendaciones usando GPT-4o-mini.

**Ejemplo**:
```bash
curl https://tu-api.com/api/places/ai-analyze
```

**Respuesta**:
```json
{
  "timestamp": "2025-11-25T10:30:00Z",
  "totalPlacesAnalyzed": 10,
  "analysis": "## 📊 Análisis de Lugares Naturales de México\n\n### Resumen General\n- Total de lugares analizados: 10\n- Categorías principales: Cascada (40%), Parque Nacional (30%), Mirador (20%), Cueva (10%)\n\n### Análisis de Accesibilidad\n- Lugares accesibles: 70%\n- No accesibles: 30%\n- Recomendación: Mejorar accesibilidad en miradores de alta elevación\n\n### Análisis de Costos\n- Promedio de entrada: $52.5 MXN\n- Lugares gratuitos: 2\n- Rango de precios: $0 - $150 MXN\n\n### Top 3 Recomendaciones\n1. **Cascadas de Agua Azul** - Ideal para familias, muy accesible\n2. **Parque Nacional Cumbres de Monterrey** - Mejores senderos\n3. **Cueva de los Cristales** - Experiencia única\n\n### Insights\n- Los lugares con más amenidades tienen mejor rating\n- La mayoría de cascadas tienen entrada económica\n- Senderos de dificultad media son los más populares"
}
```

**Características**:
- ✅ Analiza hasta 10 lugares simultáneamente
- ✅ Genera estadísticas automáticas
- ✅ Identifica patrones y tendencias
- ✅ Proporciona recomendaciones basadas en datos
- ✅ Formato Markdown para fácil visualización

---

## 🔧 Configuración

### Variables de Entorno Requeridas

**Opción 1: appsettings.json**
```json
{
  "OpenAIKey": "sk-proj-...",
  "AI": {
    "OpenAI": {
      "ApiKey": "sk-proj-...",
      "Model": "gpt-4o-mini",
      "Endpoint": "https://api.openai.com/v1/chat/completions"
    }
  }
}
```

**Opción 2: Variables de Entorno (Producción)**
```bash
OPENAI_API_KEY=sk-proj-...
AI__OpenAI__Model=gpt-4o-mini
```

**Opción 3: Docker Compose**
```yaml
environment:
  - AI__OpenAI__ApiKey=${OPENAI_API_KEY}
  - OpenAIKey=${OPENAI_API_KEY}
```

---

## 🧪 Pruebas Locales

### 1. Configurar API Key
```bash
# Editar .env
echo "OPENAI_API_KEY=sk-proj-tu-api-key-aqui" >> .env

# O editar appsettings.json directamente
```

### 2. Ejecutar la API
```bash
cd NatureAPI
dotnet run
```

### 3. Probar con el Script Automatizado
```bash
./test-api.sh
```

### 4. Probar Manualmente

**Health Check**:
```bash
curl http://localhost:5000/health
```

**Resumen con IA**:
```bash
curl http://localhost:5000/api/places/1/summary
```

**Análisis Completo**:
```bash
curl http://localhost:5000/api/places/ai-analyze
```

---

## 🐳 Despliegue con Docker

### Build Local
```bash
docker build -t nature-api -f NatureAPI/Dockerfile .
```

### Run con Variables de Entorno
```bash
docker run -p 8080:8080 \
  -e AI__OpenAI__ApiKey="sk-proj-..." \
  -e ConnectionStrings__DefaultConnection="Server=..." \
  nature-api
```

### Docker Compose Completo
```bash
docker-compose up -d
```

---

## 🚀 CI/CD con GitHub Actions

El pipeline automáticamente:

1. ✅ Compila el proyecto .NET
2. ✅ Construye imagen Docker
3. ✅ Sube a Docker Hub / GitHub Container Registry
4. ✅ Despliega a la nube elegida (Railway/Render/Azure/Fly.io)
5. ✅ Ejecuta health checks
6. ✅ Prueba endpoint de IA

**Secretos Requeridos en GitHub**:
```
OPENAI_API_KEY
DOCKER_USERNAME
DOCKER_PASSWORD
RAILWAY_TOKEN (opcional)
RENDER_DEPLOY_HOOK_URL (opcional)
API_URL (después del deploy)
```

---

## 📊 Monitoreo y Logs

### Ver Logs de IA
Los logs incluyen información detallada sobre llamadas a OpenAI:

```
[INFO] GET /api/places/1/summary
[INFO] Resumen IA generado exitosamente para lugar 1
```

```
[INFO] GET /api/places/ai-analyze - Iniciando análisis con IA
[INFO] Análisis IA completado exitosamente
```

### Fallback Automático
Si OpenAI no está disponible, la API usa un fallback local:

```
[WARNING] API Key de OpenAI no configurada. Devuelve resumen local.
```

---

## 💰 Costos Estimados OpenAI

**Modelo**: gpt-4o-mini

**Costos**:
- Input: $0.150 / 1M tokens
- Output: $0.600 / 1M tokens

**Estimación por request**:
- Resumen individual: ~500 tokens = $0.0004
- Análisis completo: ~3000 tokens = $0.002

**1000 requests/mes**: ~$2 USD

---

## 🎥 Demo para Video

### Puntos Clave a Mostrar:

1. **Código**:
   - `AiSummaryService.cs` con Azure OpenAI SDK
   - `PlacesController.cs` con endpoints de IA
   - `Prompts.cs` con templates de prompts

2. **Configuración**:
   - `appsettings.json` con API key
   - `.env` para Docker
   - Variables en GitHub Secrets

3. **Ejecución Local**:
   - `dotnet run`
   - Swagger UI en `http://localhost:5000`
   - Llamar a `/api/places/1/summary`
   - Llamar a `/api/places/ai-analyze`

4. **CI/CD**:
   - Push a GitHub
   - GitHub Actions ejecutándose
   - Build y deploy automático

5. **Producción**:
   - URL pública funcionando
   - Swagger en producción
   - Endpoints con IA respondiendo

---

## ✅ Checklist Integración IA

- [x] Paquete Azure.AI.OpenAI instalado
- [x] Paquete OpenAI oficial instalado
- [x] API Key configurada en appsettings.json
- [x] Variables de entorno configuradas
- [x] Endpoint `/api/places/{id}/summary` implementado
- [x] Endpoint `/api/places/ai-analyze` implementado ⭐
- [x] Clase Prompts.cs creada
- [x] Fallback local implementado
- [x] Logs configurados
- [x] Health checks funcionando
- [x] Docker configurado con variables de entorno
- [x] Pipeline CI/CD actualizado
- [x] Script de testing creado
- [x] Documentación completa

---

## 🆘 Troubleshooting

### Error: "OpenAI API Key no configurada"
**Solución**: Verifica que la variable `OpenAIKey` o `AI__OpenAI__ApiKey` esté configurada.

### Error: "Rate limit exceeded"
**Solución**: OpenAI tiene límites por minuto. Espera unos segundos o actualiza tu plan.

### Error: "Model not found"
**Solución**: Verifica que el modelo sea `gpt-4o-mini` o `gpt-3.5-turbo`.

### Respuesta usa fallback local
**Solución**: Verifica que la API key sea válida y tenga créditos disponibles.

---

## 📚 Referencias

- [OpenAI API Documentation](https://platform.openai.com/docs)
- [Azure OpenAI SDK](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/openai)
- [OpenAI C# SDK](https://github.com/openai/openai-dotnet)
- [GPT-4o-mini Pricing](https://openai.com/api/pricing/)

---

**¡Integración con IA completada! 🎉**

Ahora puedes desplegar y demostrar las capacidades de análisis inteligente de tu API.

