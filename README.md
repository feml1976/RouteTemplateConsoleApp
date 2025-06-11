RouteTemplateConsoleApp/
├── src/
│   ├── RouteTemplateConsoleApp/
│   │   ├── Program.cs
│   │   ├── RouteTemplateConsoleApp.csproj
│   │   └── appsettings.json
│   ├── RouteTemplateConsoleApp.Core/
│   │   ├── Models/
│   │   │   ├── RouteTemplate.cs
│   │   │   ├── Step.cs
│   │   │   ├── Location.cs
│   │   │   └── TimeInfo.cs
│   │   ├── Interfaces/
│   │   │   ├── IRouteTemplateService.cs
│   │   │   └── IApiClient.cs
│   │   └── RouteTemplateConsoleApp.Core.csproj
│   ├── RouteTemplateConsoleApp.Infrastructure/
│   │   ├── Services/
│   │   │   ├── RouteTemplateService.cs
│   │   │   └── FrotcomApiClient.cs
│   │   ├── Configuration/
│   │   │   └── ApiConfiguration.cs
│   │   └── RouteTemplateConsoleApp.Infrastructure.csproj
│   └── RouteTemplateConsoleApp.Application/
│       ├── Services/
│       │   └── RouteDisplayService.cs
│       ├── Interfaces/
│       │   └── IRouteDisplayService.cs
│       └── RouteTemplateConsoleApp.Application.csproj
├── tests/
│   └── RouteTemplateConsoleApp.Tests/
│       ├── Services/
│       │   ├── RouteTemplateServiceTests.cs
│       │   └── FrotcomApiClientTests.cs
│       └── RouteTemplateConsoleApp.Tests.csproj
├── README.md
└── RouteTemplateConsoleApp.sln

# RouteTemplateConsoleApp

## Descripción

**RouteTemplateConsoleApp** es una aplicación de consola desarrollada en **.NET 9.0** que consume la API de Frotcom para obtener y mostrar plantillas de rutas logísticas. La aplicación está diseñada con una arquitectura por capas y utiliza inyección de dependencias para mantener un código limpio, mantenible y testeable.

## 🚀 Características Principales

- **Consumo asíncrono de API REST** usando HttpClient
- **Arquitectura por capas** (Core, Infrastructure, Application)
- **Inyección de dependencias** con Microsoft.Extensions.DependencyInjection
- **Logging estructurado** con Microsoft.Extensions.Logging
- **Manejo robusto de errores** con bloques try-catch
- **Documentación XML** completa en clases y métodos
- **Configuración externa** mediante appsettings.json
- **Interfaz de consola amigable** con emojis y formato estructurado

## 🏗️ Arquitectura del Proyecto

```
RouteTemplateConsoleApp/
├── 📁 Core/                    # Capa de dominio
│   ├── Models/                 # Entidades de dominio
│   └── Interfaces/             # Contratos de servicios
├── 📁 Infrastructure/          # Capa de infraestructura
│   ├── Services/               # Implementaciones de servicios
│   └── Configuration/          # Configuraciones
├── 📁 Application/             # Capa de aplicación
│   ├── Services/               # Servicios de aplicación
│   └── Interfaces/             # Contratos de aplicación
└── 📁 ConsoleApp/              # Punto de entrada
    ├── Program.cs              # Configuración e inicio
    └── appsettings.json        # Configuración de la app
```

## 🛠️ Tecnologías Utilizadas

- **.NET 9.0** - Framework de desarrollo
- **Microsoft.Extensions.Hosting** - Host genérico para aplicaciones
- **Microsoft.Extensions.DependencyInjection** - Inyección de dependencias
- **Microsoft.Extensions.Configuration** - Manejo de configuración
- **Microsoft.Extensions.Logging** - Sistema de logging
- **Microsoft.Extensions.Http** - Cliente HTTP con factory pattern
- **System.Text.Json** - Serialización/deserialización JSON

## 📦 Instalación y Configuración

### Prerrequisitos

- **.NET 9.0 SDK** instalado
- Conexión a internet para consumir la API
- IDE compatible (Visual Studio, VS Code, Rider)

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone <repository-url>
   cd RouteTemplateConsoleApp
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Configurar la aplicación**
   
   Editar `appsettings.json` si es necesario:
   ```json
   {
     "FrotcomApi": {
       "BaseUrl": "https://v2api.frotcom.com/v2/routes/templatesWithSteps",
       "ApiKey": "da9c8b2c-f9e6-445a-9b9f-c31b1250ce4a",
       "TimeoutSeconds": 30
     }
   }
   ```

4. **Compilar la aplicación**
   ```bash
   dotnet build
   ```

5. **Ejecutar la aplicación**
   ```bash
   dotnet run --project src/RouteTemplateConsoleApp
   ```

## 🎯 Funcionalidades

### 1. Obtención de Datos
- Consume la API de Frotcom de forma asíncrona
- Maneja timeouts y errores de red
- Deserializa JSON a objetos tipados

### 2. Procesamiento de Información
- Calcula distancias totales de rutas
- Formatea duraciones en formato legible
- Agrupa información por usuario

### 3. Visualización en Consola
- Muestra información detallada de cada ruta
- Presenta resúmenes estadísticos
- Utiliza emojis y formato estructurado para mejor legibilidad

### 4. Manejo de Errores
- Logging detallado de errores
- Manejo graceful de fallos de API
- Mensajes de error descriptivos para el usuario

## 🔧 Configuración Avanzada

### Variables de Entorno
La aplicación puede configurarse mediante variables de entorno:

```bash
export FrotcomApi__ApiKey="tu-api-key"
export FrotcomApi__TimeoutSeconds="60"
```

### Niveles de Logging
Configurar en `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "RouteTemplateConsoleApp": "Debug",
      "Microsoft": "Warning"
    }
  }
}
```

## 📊 Ejemplo de Salida

```
╔══════════════════════════════════════════════════════════════╗
║                    PLANTILLAS DE RUTAS                      ║
╚══════════════════════════════════════════════════════════════╝

🚛 RUTA 1 de 4
────────────────────────────────────────────────────────────────
📋 ID: 3441338
🏷️  Nombre: Bav3-autonorte-melgar-calarca-circasi paila-yumbo2
🔗 Código: R034
📍 Origen: BAVARIA TOCANCIPA-ESPERA, CUNDINAMARCA
🎯 Destino: BAVARIA CALI, VALLE DEL CAUCA
⏱️  Duración: 1d 0h 0m
📏 Distancia Total: 546,121 metros (546.1 km)
🔢 Número de Segmentos: 11
👤 Usuario: TRANSERADM
📅 Fecha: 2023-09-29 15:53:50

📋 SEGMENTOS DE LA RUTA:
   01. BAV TOCANCIPA-ESPERA → BAVARIA TOCANCIPA
       📏 815m | ⏱️ 4m 27s
   02. BAVARIA TOCANCIPA → CLL 170 CON AUTONORTE
       📏 29,391m | ⏱️ 35m 16s
   ...
```

## 🧪 Testing

### Ejecutar Tests
```bash
dotnet test
```

### Cobertura de Tests
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Convenciones de Código

- **Nomenclatura**: PascalCase para métodos y propiedades, camelCase para variables locales
- **Documentación**: XML documentation en todos los métodos públicos
- **Async/Await**: Usar sufijo "Async" en métodos asíncronos
- **Logging**: Usar structured logging con niveles apropiados
- **Exception Handling**: Catch específico antes que genérico

## 🐛 Troubleshooting

### Error: "Unable to connect to the API"
- Verificar conexión a internet
- Validar que la API key sea correcta
- Comprobar que la URL de la API esté disponible

### Error: "JSON deserialization failed"
- Verificar que el formato de respuesta no haya cambiado
- Revisar logs para detalles específicos del error

### Error: "Timeout occurred"
- Aumentar el valor de `TimeoutSeconds` en configuración
- Verificar estabilidad de la conexión de red

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para detalles.

## 👥 Autores

- **Desarrollador Principal** - *Trabajo inicial* - [Tu GitHub](https://github.com/tuusuario)

## 🙏 Agradecimientos

- Frotcom por proporcionar la API de plantillas de rutas
- Microsoft por el excelente framework .NET
- Comunidad de desarrolladores .NET por las mejores prácticas

---

## 📞 Soporte

Para soporte técnico o preguntas:
- 📧 Email: soporte@tudominio.com
- 🐛 Issues: [GitHub Issues](https://github.com/tuusuario/RouteTemplateConsoleApp/issues)
- 📚 Wiki: [GitHub Wiki](https://github.com/tuusuario/RouteTemplateConsoleApp/wiki)