#!/bin/bash

# Script de prueba local para NatureAPI con OpenAI

echo "🧪 Testing NatureAPI - Integración OpenAI"
echo "=========================================="
echo ""

# Colores
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# URL base (cambia según tu entorno)
BASE_URL="${API_URL:-http://localhost:5000}"

echo "📍 Base URL: $BASE_URL"
echo ""

# Test 1: Health Check
echo "1️⃣ Health Check..."
response=$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/health")
if [ "$response" = "200" ]; then
    echo -e "${GREEN}✅ Health check OK${NC}"
else
    echo -e "${RED}❌ Health check failed (HTTP $response)${NC}"
fi
echo ""

# Test 2: Swagger UI
echo "2️⃣ Swagger UI..."
response=$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/")
if [ "$response" = "200" ]; then
    echo -e "${GREEN}✅ Swagger UI accesible${NC}"
else
    echo -e "${YELLOW}⚠️  Swagger UI no accesible (HTTP $response)${NC}"
fi
echo ""

# Test 3: Listar lugares
echo "3️⃣ GET /api/places..."
places=$(curl -s "$BASE_URL/api/places")
if echo "$places" | grep -q "id"; then
    count=$(echo "$places" | grep -o '"id"' | wc -l)
    echo -e "${GREEN}✅ Lugares obtenidos: $count${NC}"
else
    echo -e "${RED}❌ Error obteniendo lugares${NC}"
fi
echo ""

# Test 4: Obtener lugar específico
echo "4️⃣ GET /api/places/1..."
place=$(curl -s "$BASE_URL/api/places/1")
if echo "$place" | grep -q "name"; then
    place_name=$(echo "$place" | grep -o '"name":"[^"]*"' | cut -d'"' -f4)
    echo -e "${GREEN}✅ Lugar obtenido: $place_name${NC}"
else
    echo -e "${RED}❌ Error obteniendo lugar${NC}"
fi
echo ""

# Test 5: Integración OpenAI (Punto clave para el examen)
echo "5️⃣ GET /api/places/1/summary (OpenAI)..."
echo -e "${YELLOW}⏳ Generando resumen con IA...${NC}"
summary=$(curl -s "$BASE_URL/api/places/1/summary")

if echo "$summary" | grep -q "summary"; then
    echo -e "${GREEN}✅ Resumen IA generado exitosamente!${NC}"
    echo ""
    echo "📝 Resumen:"
    echo "$summary" | grep -o '"summary":"[^"]*"' | cut -d'"' -f4 | fold -w 70
    echo ""
    
    # Verificar si usa OpenAI real o fallback
    if echo "$summary" | grep -q "Elevación"; then
        echo -e "${YELLOW}⚠️  Usando fallback local (OpenAI no configurado)${NC}"
    else
        echo -e "${GREEN}🎉 ¡OpenAI funcionando correctamente!${NC}"
    fi
else
    echo -e "${RED}❌ Error generando resumen IA${NC}"
    echo "Respuesta: $summary"
fi
echo ""

# Test 6: Verificar variables de entorno (solo en local)
if [ "$BASE_URL" = "http://localhost:5000" ] || [ "$BASE_URL" = "http://localhost:8080" ]; then
    echo "6️⃣ Verificando configuración local..."
    if [ -f ".env" ]; then
        if grep -q "OPENAI_API_KEY=sk-" .env; then
            echo -e "${GREEN}✅ OPENAI_API_KEY configurada en .env${NC}"
        else
            echo -e "${YELLOW}⚠️  OPENAI_API_KEY no configurada en .env${NC}"
            echo -e "${YELLOW}   Agrega tu API key: OPENAI_API_KEY=sk-...${NC}"
        fi
    else
        echo -e "${YELLOW}⚠️  Archivo .env no encontrado${NC}"
    fi
    echo ""
fi

# Resumen final
echo "=========================================="
echo "✨ Tests completados"
echo ""
echo "📚 Para configurar OpenAI:"
echo "   1. Obtén tu API key: https://platform.openai.com/api-keys"
echo "   2. Agrégala en .env: OPENAI_API_KEY=sk-..."
echo "   3. O en appsettings.json: AI.OpenAI.ApiKey"
echo ""
echo "🚀 Para desplegar a producción, sigue: DEPLOYMENT.md"
echo ""

