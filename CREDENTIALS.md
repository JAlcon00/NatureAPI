# 🔐 Configuración de Credenciales - NatureAPI

## 📋 Credenciales de Base de Datos

### **Credenciales Actuales:**
- **Servidor**: `localhost:1433`
- **Base de datos**: `NatureDB`
- **Usuario**: `SA`
- **Contraseña**: `Password123!`

## 🚀 Configuración Rápida

### **1. Variables de Entorno (.env)**
Copia el archivo `.env` que está en la raíz del proyecto:
```bash
SA_PASSWORD=Password123!
MSSQL_PID=Express
```

### **2. Configuración de Aplicación**
El archivo `appsettings.json` contiene:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NatureDB;User Id=SA;Password=Password123!;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

## 🐳 Docker Compose

### **Ejecutar SQL Server:**
```bash
# Levantar SQL Server con Docker
docker-compose up -d

# Verificar que esté funcionando
docker-compose logs sqlserver
```

### **Conectar desde la API:**
```bash
# Ejecutar la API
cd NatureAPI
dotnet run
```

## 🛡️ Seguridad Implementada

- ✅ **Archivo `.env`** protegido en `.gitignore`
- ✅ **Variables de entorno** en Docker Compose
- ✅ **Archivos de plantilla** para otros desarrolladores
- ✅ **Configuración separada** por ambiente

## 🔧 Para Nuevos Desarrolladores

1. Copia `docker-compose.example.yml` a `docker-compose.yml`
2. Copia `appsettings.example.json` a `appsettings.json`
3. Configura tus propias credenciales
4. Crea tu archivo `.env` con las variables necesarias

## ⚠️ Importante

- **NUNCA** subas archivos con credenciales reales al repositorio
- **USA** siempre variables de entorno en producción
- **CAMBIA** las credenciales por defecto en producción
