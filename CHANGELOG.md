# Changelog

Todos los cambios notables en este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere a [Semantic Versioning](https://semver.org/lang/es/).

## [Unreleased]

### Planeado
- Implementación de servicios de API (Timbrado, Cancelación, Consultas)
- Soporte para CFDI 3.3
- Complementos de CFDI (Pagos, Nómina, etc.)
- Herramientas de CLI

## [1.0.0] - 2026-04-09

### Agregado
- ✨ SDK inicial para TecnoFact .NET
- ✨ Enumeración `TecnoFactEnvironment` con métodos de extensión para Sandbox/Production
- ✨ Enumeración `TipoComprobante` para tipos de CFDI (Ingreso, Egreso, Traslado, Nómina, Pago)
- ✨ Sistema de excepciones personalizadas:
  - `TecnoFactException` (base)
  - `AuthenticationException`
  - `ValidationException`
  - `TimbradoException`
  - `CancelacionException`
  - `NotFoundException`
  - `RateLimitException`
  - `ServerException`
- ✨ Modelos de datos completos para CFDI 4.0:
  - `Emisor` - Datos del emisor
  - `Receptor` - Datos del receptor
  - `Concepto` - Conceptos/partidas de factura
  - `Traslado` y `Retencion` - Impuestos por concepto
  - `TrasladoGlobal` y `RetencionGlobal` - Impuestos globales
  - `Impuestos` - Contenedor de impuestos
  - `ImpuestosConcepto` - Impuestos por concepto
  - `CfdiRelacionados` - CFDIs relacionados
  - `CuentaPredial` - Información de cuenta predial
  - `InformacionAduanera` - Información aduanera
  - `Parte` - Partes/componentes de conceptos
  - `Cfdi4Request` - Solicitud completa de CFDI 4.0
- ✨ Clase `TecnoFactConfig` inmutable con validaciones
- ✨ Cliente HTTP `TecnoFactHttpClient` con:
  - Autenticación Basic
  - Reintentos automáticos configurables
  - Manejo robusto de errores
  - Serialización JSON con snake_case
  - Soporte para GET, POST, PUT, DELETE, PATCH
- ✨ Interfaz `IHttpClient` para inyección de dependencias
- ✨ 22 tests unitarios con xUnit
- ✨ Ejemplo de uso básico
- 📝 Documentación completa en español (README.md)
- 📝 Guía de contribución (CONTRIBUTING.md)
- 📝 Licencia MIT
- 🔧 Configuración de proyecto para .NET 10.0
- 🔧 Metadatos de NuGet package
- 🔧 GitHub Actions para CI/CD:
  - Workflow de tests automáticos (CI)
  - Workflow de release y publicación a NuGet
  - Análisis de seguridad con CodeQL
- 🔧 Dependabot para actualizaciones automáticas

### Características Técnicas
- 🎯 Tipado fuerte con nullable reference types
- 🎯 API completamente asíncrona (async/await)
- 🎯 Documentación XML integrada
- 🎯 Soporte multiplataforma (Windows, Linux, macOS)
- 🎯 Compatible con .NET 10.0+
- 🎯 Serialización JSON nativa con System.Text.Json
- 🎯 Patrón de configuración inmutable
- 🎯 Métodos de extensión para enums

### Seguridad
- 🔒 Análisis de código estático con CodeQL
- 🔒 Actualizaciones automáticas de dependencias con Dependabot
- 🔒 Validación de entrada en constructores
- 🔒 Manejo seguro de credenciales (no hardcoded)

## [0.1.0] - 2026-04-09

### Agregado
- 🎉 Versión inicial del proyecto
- 📁 Estructura de carpetas del SDK
- 🏗️ Configuración básica del proyecto .NET

---

## Tipos de Cambios

- `Agregado` - Para nuevas funcionalidades
- `Cambiado` - Para cambios en funcionalidades existentes
- `Obsoleto` - Para funcionalidades que serán removidas
- `Removido` - Para funcionalidades removidas
- `Corregido` - Para corrección de bugs
- `Seguridad` - Para vulnerabilidades de seguridad

## Enlaces

- [Unreleased]: https://github.com/TecnoFact/SDK-tecnofact-net/compare/v1.0.0...HEAD
- [1.0.0]: https://github.com/TecnoFact/SDK-tecnofact-net/releases/tag/v1.0.0
- [0.1.0]: https://github.com/TecnoFact/SDK-tecnofact-net/releases/tag/v0.1.0
