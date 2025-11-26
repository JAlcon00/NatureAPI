namespace NatureAPI.Services;

public static class Prompts
{
    public static string GeneratePlacesAnalysisPrompt(string jsonData)
    {
        return $@"Eres un experto analista de datos de turismo natural en México. 
Analiza los siguientes datos de lugares naturales y proporciona insights valiosos:

DATOS:
{jsonData}

Por favor, proporciona un análisis detallado que incluya:

1. **Resumen General**: Cantidad total de lugares, categorías más comunes
2. **Análisis de Accesibilidad**: Porcentaje de lugares accesibles vs no accesibles
3. **Análisis de Costos**: Rango de precios de entrada, promedio, lugares gratuitos
4. **Análisis de Senderos**: Dificultades más comunes, distancias promedio
5. **Recomendaciones**: Top 3 lugares recomendados según características únicas
6. **Insights**: Patrones interesantes o tendencias observadas

Formatea la respuesta de manera clara y profesional en español.";
    }

    public static string GeneratePlaceSummaryPrompt(string placeName, string category, 
        int elevationMeters, bool accessible, decimal entryFee, 
        List<string> trails, List<string> amenities)
    {
        return $@"Genera un resumen breve y atractivo (máximo 150 palabras) en español sobre el siguiente lugar natural de México:

**Nombre**: {placeName}
**Categoría**: {category}
**Elevación**: {elevationMeters} metros
**Accesible**: {(accessible ? "Sí" : "No")}
**Costo de entrada**: ${entryFee} MXN
**Senderos**: {string.Join(", ", trails)}
**Amenidades**: {string.Join(", ", amenities)}

El resumen debe ser informativo, atractivo para turistas y destacar las características únicas del lugar.
Incluye información sobre qué esperar, mejor época para visitar, y consejos útiles.";
    }

    public static string GenerateRecommendationsPrompt(string userPreferences)
    {
        return $@"Basándote en las siguientes preferencias del usuario, recomienda lugares naturales de México apropiados:

{userPreferences}

Proporciona al menos 3 recomendaciones específicas con justificación de por qué cada lugar es adecuado.";
    }
}

