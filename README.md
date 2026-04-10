# TecnoFact SDK para Facturación Electrónica CFDI 4.0 (.NET)

<div align="center">

[![NuGet Version](https://img.shields.io/nuget/v/TecnoFact.SDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/TecnoFact.SDK/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/TecnoFact.SDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/TecnoFact.SDK/)
[![CI Build](https://img.shields.io/github/actions/workflow/status/TecnoFact/SDK-tecnofact-net/ci.yml?branch=main&style=flat-square&logo=github&label=CI)](https://github.com/TecnoFact/SDK-tecnofact-net/actions/workflows/ci.yml)
[![CodeQL](https://img.shields.io/github/actions/workflow/status/TecnoFact/SDK-tecnofact-net/codeql.yml?branch=main&style=flat-square&logo=github&label=CodeQL)](https://github.com/TecnoFact/SDK-tecnofact-net/actions/workflows/codeql.yml)
[![codecov](https://img.shields.io/codecov/c/github/TecnoFact/SDK-tecnofact-net?style=flat-square&logo=codecov)](https://codecov.io/gh/TecnoFact/SDK-tecnofact-net)

[![.NET Version](https://img.shields.io/badge/.NET-10.0+-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C# Version](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License: MIT](https://img.shields.io/github/license/TecnoFact/SDK-tecnofact-net?style=flat-square)](https://opensource.org/licenses/MIT)
[![GitHub Release](https://img.shields.io/github/v/release/TecnoFact/SDK-tecnofact-net?style=flat-square&logo=github)](https://github.com/TecnoFact/SDK-tecnofact-net/releases)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](CONTRIBUTING.md)

[![Maintainability](https://img.shields.io/codeclimate/maintainability/TecnoFact/SDK-tecnofact-net?style=flat-square&logo=codeclimate)](https://codeclimate.com/github/TecnoFact/SDK-tecnofact-net)
[![Security Rating](https://img.shields.io/badge/security-A+-success?style=flat-square&logo=security)](https://github.com/TecnoFact/SDK-tecnofact-net/security)
[![Dependabot](https://img.shields.io/badge/Dependabot-enabled-success?style=flat-square&logo=dependabot)](https://github.com/TecnoFact/SDK-tecnofact-net/network/updates)

</div>

SDK oficial de .NET para la integración con el servicio de Timbrado CFDI 4.0 de TecnoFact. Facilita la emisión, cancelación y consulta de facturas electrónicas cumpliendo con los requisitos del SAT mexicano.

## 📋 Tabla de Contenidos

- [Características](#características)
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Estructura del SDK](#estructura-del-sdk)
- [Uso Básico](#uso-básico)
- [Modelos de Datos](#modelos-de-datos)
- [Manejo de Errores](#manejo-de-errores)
- [Testing](#testing)
- [Contribuciones](#contribuciones)
- [Soporte](#soporte)
- [Licencia](#licencia)

## ✨ Características

- **Timbrado CFDI 4.0**: Emisión de facturas electrónicas cumpliendo con la versión 4.0 del CFDI
- **Timbrado CFDI 3.3**: Soporte retroactivo para facturación CFDI 3.3
- **Cancelación**: Cancelación de CFDIs con diferentes motivos
- **Consultas**: Búsqueda y recuperación de CFDIs timbrados
- **Reportes**: Generación de reportes y estadísticas
- **Validaciones**: Validación de RFCs y catálogos del SAT
- **Health Checks**: Verificación del estado de servicios
- **Tipado Fuerte**: Compatible con .NET 10.0+ con nullable reference types
- **Async/Await**: API completamente asíncrona
- **Manejo de Errores**: Sistema robusto de excepciones personalizadas

## 🔧 Requisitos

- **.NET**: >= 10.0
- **C#**: >= 12.0

## 📦 Instalación

### Usando NuGet Package Manager

```bash
dotnet add package TecnoFact.SDK
```

### Usando Package Manager Console

```powershell
Install-Package TecnoFact.SDK
```

### Desde el código fuente

```bash
git clone https://github.com/TecnoFact/SDK-tecnofact-net.git
cd SDK-tecnofact-net
dotnet build
```

## ⚙️ Configuración

### Constructor Directo

```csharp
using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;

var config = new TecnoFactConfig(
    apiKey: "TU_API_KEY",
    apiSecret: "TU_API_SECRET",
    environment: TecnoFactEnvironment.Sandbox,
    timeout: 30,
    retries: 3
);

Console.WriteLine($"Entorno: {config.GetEnvironment().Label()}");
Console.WriteLine($"URL Base: {config.GetBaseUrl()}");
Console.WriteLine($"Timeout: {config.GetTimeout()} segundos");
```

### Variables de Entorno

Crea un archivo `.env` o configura las variables de entorno:

```env
TECNOFACT_API_KEY=tu_api_key
TECNOFACT_API_SECRET=tu_api_secret
TECNOFACT_ENVIRONMENT=Sandbox
TECNOFACT_TIMEOUT=30
```

```csharp
using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;

var apiKey = System.Environment.GetEnvironmentVariable("TECNOFACT_API_KEY");
var apiSecret = System.Environment.GetEnvironmentVariable("TECNOFACT_API_SECRET");
var envString = System.Environment.GetEnvironmentVariable("TECNOFACT_ENVIRONMENT") ?? "Sandbox";
var timeout = int.Parse(System.Environment.GetEnvironmentVariable("TECNOFACT_TIMEOUT") ?? "30");

var environment = Enum.Parse<TecnoFactEnvironment>(envString);

var config = new TecnoFactConfig(
    apiKey: apiKey!,
    apiSecret: apiSecret!,
    environment: environment,
    timeout: timeout
);
```

## 🏗️ Estructura del SDK

```
TecnoFact.SDK/
├── Config/
│   └── TecnoFactConfig.cs          # Configuración inmutable del SDK
├── Contracts/
│   └── IHttpClient.cs              # Interfaz para el cliente HTTP
├── Enums/
│   ├── TecnoFactEnvironment.cs     # Entornos (Sandbox/Production)
│   └── TipoComprobante.cs          # Tipos de CFDI
├── Exceptions/
│   ├── TecnoFactException.cs       # Excepción base
│   ├── AuthenticationException.cs  # Error de autenticación
│   ├── ValidationException.cs      # Error de validación
│   ├── TimbradoException.cs        # Error de timbrado
│   ├── CancelacionException.cs     # Error de cancelación
│   ├── NotFoundException.cs        # Recurso no encontrado
│   ├── RateLimitException.cs       # Límite de peticiones
│   └── ServerException.cs          # Error del servidor
├── Http/
│   └── TecnoFactHttpClient.cs      # Cliente HTTP con HttpClient
└── Models/
    ├── Emisor.cs                   # Datos del emisor
    ├── Receptor.cs                 # Datos del receptor
    ├── Concepto.cs                 # Conceptos de factura
    ├── Cfdi4Request.cs             # Solicitud CFDI 4.0
    ├── CfdiRelacionados.cs         # CFDIs relacionados
    ├── Impuestos.cs                # Impuestos globales
    ├── ImpuestosConcepto.cs        # Impuestos por concepto
    ├── Traslado.cs                 # Traslado de impuestos
    ├── TrasladoGlobal.cs           # Traslado global
    ├── Retencion.cs                # Retención de impuestos
    ├── RetencionGlobal.cs          # Retención global
    ├── CuentaPredial.cs            # Cuenta predial
    ├── InformacionAduanera.cs      # Información aduanera
    └── Parte.cs                    # Partes/componentes
```

## 💻 Uso Básico

### Ejemplo: Crear Configuración

```csharp
using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;

var config = new TecnoFactConfig(
    apiKey: "TU_API_KEY",
    apiSecret: "TU_API_SECRET",
    environment: TecnoFactEnvironment.Sandbox,
    timeout: 30,
    retries: 3
);

Console.WriteLine($"Entorno: {config.GetEnvironment().Label()}");
Console.WriteLine($"URL Base: {config.GetBaseUrl()}");
Console.WriteLine($"Timeout: {config.GetTimeout()} segundos");

// Convertir a diccionario
var data = config.ToDictionary();
foreach (var kvp in data)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

### Ejemplo: Enum TecnoFactEnvironment

```csharp
using TecnoFact.SDK.Enums;

// Usar enum con IntelliSense
var env = TecnoFactEnvironment.Production;

if (env.IsProduction())
{
    Console.WriteLine("Entorno de producción");
}

// Métodos del enum
Console.WriteLine(env.Value());           // 'production'
Console.WriteLine(env.IsProduction());    // True
Console.WriteLine(env.IsSandbox());       // False
Console.WriteLine(env.Label());           // 'Producción'
Console.WriteLine(env.GetBaseUrl());      // URL del entorno
```

## 📋 Modelos de Datos

### Emisor

```csharp
using TecnoFact.SDK.Models;

var emisor = new Emisor(
    rfc: "XAXX010101000",
    nombre: "EMPRESA EMISORA SA DE CV",
    regimenFiscal: "601",
    cp: "06300"
);

Console.WriteLine(emisor.GetRfc());     // XAXX010101000
Console.WriteLine(emisor.GetNombre());  // EMPRESA EMISORA SA DE CV
var dict = emisor.ToDictionary();
```

### Receptor

```csharp
using TecnoFact.SDK.Models;

var receptor = new Receptor(
    rfc: "XAXX010101001",
    nombre: "CLIENTE RECEPTOR",
    usoCfdi: "G03",
    domicilioFiscalReceptor: "06300",
    regimenFiscalReceptor: "612"
);
```

### Concepto con Impuestos

```csharp
using TecnoFact.SDK.Models;

var concepto = new Concepto(
    claveProdServ: "01010101",
    cantidad: 1m,
    claveUnidad: "E48",
    descripcion: "Servicio de desarrollo de software",
    valorUnitario: 10000.00m,
    importe: 10000.00m,
    objetoImp: "02",  // Sí objeto de impuesto
    impuestos: new ImpuestosConcepto(
        traslados: new List<Traslado>
        {
            new Traslado(
                baseImporte: 10000.00m,
                impuesto: "002",  // IVA
                tipoFactor: "Tasa",
                tasaOCuota: 0.160000m,
                importe: 1600.00m
            )
        }
    )
);
```

### Solicitud CFDI 4.0

```csharp
using TecnoFact.SDK.Models;

var request = new Cfdi4Request(
    emisor: emisor,
    receptor: receptor,
    conceptos: new List<Concepto> { concepto }
)
{
    TipoComprobante = "I",
    FormaPago = "01",
    MetodoPago = "PUE",
    Moneda = "MXN",
    Subtotal = 10000.00m,
    Total = 11600.00m
};
```

## ⚠️ Manejo de Errores

El SDK proporciona excepciones específicas para diferentes tipos de errores:

```csharp
using TecnoFact.SDK.Exceptions;
using TecnoFact.SDK.Http;

try
{
    var httpClient = new TecnoFactHttpClient(config);
    // Tu código aquí
}
catch (AuthenticationException ex)
{
    Console.WriteLine($"Error de autenticación: {ex.Message}");
    var details = ex.GetDetails();
    foreach (var detail in details)
    {
        Console.WriteLine($"{detail.Key}: {detail.Value}");
    }
}
catch (ValidationException ex)
{
    Console.WriteLine($"Error de validación: {ex.Message}");
}
catch (TimbradoException ex)
{
    Console.WriteLine($"Error en timbrado: {ex.Message}");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Recurso no encontrado: {ex.Message}");
}
catch (RateLimitException ex)
{
    Console.WriteLine($"Límite de peticiones excedido: {ex.Message}");
}
catch (ServerException ex)
{
    Console.WriteLine($"Error del servidor: {ex.Message}");
}
catch (TecnoFactException ex)
{
    Console.WriteLine($"Error general: {ex.Message}");
}
```

## 🧪 Testing

### Ejecutar Tests

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar con cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Ejecutar tests específicos
dotnet test --filter "FullyQualifiedName~ConfigTests"
```

### Análisis Estático

```bash
# Restaurar herramientas
dotnet tool restore

# Análisis de código
dotnet build /p:TreatWarningsAsErrors=true

# Formateo de código
dotnet format
```

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Estándares de Código

- Seguir las convenciones de C# y .NET
- Usar nullable reference types
- Escribir documentación XML
- Mantener cobertura de tests > 80%
- Pasar todos los checks de análisis estático

## 💬 Soporte

- **Email**: soporte@tecnofact.com
- **Documentación**: https://docs.tecnofact.com
- **Issues**: https://github.com/TecnoFact/SDK-tecnofact-net/issues

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 🏢 Sobre TecnoFact

TecnoFact es un proveedor autorizado de certificación (PAC) que ofrece servicios de timbrado de CFDI cumpliendo con todos los requisitos del SAT mexicano.

### Características del Servicio

- ✅ PAC Autorizado por el SAT
- ✅ Disponibilidad 99.9%
- ✅ Soporte técnico especializado
- ✅ Precios competitivos
- ✅ API REST moderna
- ✅ Documentación completa
- ✅ SDKs en múltiples lenguajes

---

Desarrollado con ❤️ por TecnoFact
