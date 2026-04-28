# 🧪 Guía de Testing - TecnoFact SDK

Documentación completa sobre testing, cobertura de código y mejores prácticas para el SDK de TecnoFact.

## 📋 Tabla de Contenidos

- [Resumen de Tests](#resumen-de-tests)
- [Ejecutar Tests](#ejecutar-tests)
- [Cobertura de Código](#cobertura-de-código)
- [Tipos de Tests](#tipos-de-tests)
- [Estructura de Tests](#estructura-de-tests)
- [Mejores Prácticas](#mejores-prácticas)
- [CI/CD](#cicd)

## 📊 Resumen de Tests

### Estadísticas Actuales

```
✅ Tests Totales: 62
✅ Tests Pasando: 62
✅ Cobertura: ~85%+ (objetivo: 90%)
✅ Tiempo de Ejecución: ~22s
```

### Distribución por Categoría

| Categoría | Tests | Descripción |
|-----------|-------|-------------|
| **ConfigTests** | 9 | Tests de configuración del SDK |
| **EnumTests** | 12 | Tests de enumeraciones |
| **ModelTests** | 9 | Tests de modelos de datos |
| **ExceptionTests** | 10 | Tests de excepciones personalizadas |
| **HttpClientTests** | 10 | Tests del cliente HTTP |
| **ModelValidationTests** | 12 | Tests de validación de modelos |

## 🚀 Ejecutar Tests

### Localmente (sin Docker)

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar con logs detallados
dotnet test --logger "console;verbosity=detailed"

# Ejecutar tests específicos
dotnet test --filter "FullyQualifiedName~ConfigTests"
dotnet test --filter "FullyQualifiedName~HttpClientTests"

# Ejecutar en modo watch (auto-reload)
dotnet watch test
```

### Con Docker (Recomendado)

```bash
# Ejecutar todos los tests en Docker
make docker-test

# O directamente con docker-compose
docker-compose run --rm sdk-tests

# Modo desarrollo con hot reload
make docker-dev
docker-compose up sdk-dev
```

### Con Makefile

```bash
# Ver comandos disponibles
make help

# Tests locales
make test           # Ejecutar tests
make test-watch     # Watch mode
make coverage       # Con cobertura

# Tests en Docker
make docker-test         # Tests en Docker
make docker-coverage     # Cobertura en Docker
make docker-integration  # Tests de integración
```

## 📈 Cobertura de Código

### Generar Reporte de Cobertura

#### Localmente

```bash
# Generar cobertura
make coverage

# O manualmente
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Ver archivos generados
ls coverage/**/coverage.cobertura.xml
```

#### Con Docker

```bash
# Generar reporte HTML completo
make docker-coverage

# Abrir reporte en navegador
# Windows
start coverage-report/index.html

# macOS
open coverage-report/index.html

# Linux
xdg-open coverage-report/index.html
```

### Archivos de Cobertura

```
coverage/
├── {guid}/
│   └── coverage.cobertura.xml    # Datos de cobertura
└── coverage-report/
    ├── index.html                # Reporte HTML principal
    ├── badge_combined.svg        # Badge de cobertura
    └── Summary.txt               # Resumen en texto
```

### Integración con Codecov

El proyecto está configurado para subir cobertura a Codecov automáticamente en CI:

```yaml
# .github/workflows/ci.yml
- name: Upload coverage to Codecov
  uses: codecov/codecov-action@v4
  with:
    files: ./coverage/**/coverage.cobertura.xml
```

## 🧪 Tipos de Tests

### 1. Tests Unitarios

Prueban componentes individuales de forma aislada.

**Ejemplo:**

```csharp
[Fact]
public void Constructor_WithValidParameters_CreatesConfig()
{
    var config = new TecnoFactConfig(
        apiKey: "test_key",
        apiSecret: "test_secret",
        environment: TecnoFactEnvironment.Sandbox
    );

    Assert.Equal("test_key", config.ApiKey);
    Assert.Equal("test_secret", config.ApiSecret);
}
```

### 2. Tests de Validación

Verifican que las validaciones funcionen correctamente.

**Ejemplo:**

```csharp
[Fact]
public void Constructor_WithEmptyApiKey_ThrowsArgumentException()
{
    Assert.Throws<ArgumentException>(() =>
        new TecnoFactConfig("", "secret", TecnoFactEnvironment.Sandbox)
    );
}
```

### 3. Tests Parametrizados

Prueban múltiples casos con diferentes datos.

**Ejemplo:**

```csharp
[Theory]
[InlineData("G01")]
[InlineData("G02")]
[InlineData("G03")]
public void Receptor_WithValidUsoCfdi_CreatesReceptor(string usoCfdi)
{
    var receptor = new Receptor("XAXX010101000", "Test", usoCfdi);
    Assert.Equal(usoCfdi, receptor.UsoCfdi);
}
```

### 4. Tests de Integración (Futuro)

Prueban la integración con servicios externos usando mock API.

```bash
# Ejecutar tests de integración
make docker-integration
```

## 📁 Estructura de Tests

```
TecnoFact.SDK.Tests/
├── ConfigTests.cs              # Tests de configuración
├── EnumTests.cs                # Tests de enumeraciones
├── ModelTests.cs               # Tests de modelos básicos
├── ModelValidationTests.cs     # Tests de validación de modelos
├── ExceptionTests.cs           # Tests de excepciones
├── HttpClientTests.cs          # Tests del cliente HTTP
└── TecnoFact.SDK.Tests.csproj  # Configuración del proyecto
```

### Dependencias de Testing

```xml
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="coverlet.collector" Version="6.0.4" />
<PackageReference Include="coverlet.msbuild" Version="6.0.4" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="FluentAssertions" Version="6.12.2" />
```

## ✅ Mejores Prácticas

### Nomenclatura de Tests

```csharp
// Patrón: MethodName_Scenario_ExpectedBehavior
[Fact]
public void Constructor_WithEmptyApiKey_ThrowsArgumentException()

[Fact]
public void GetBaseUrl_WithSandbox_ReturnsSandboxUrl()

[Fact]
public void ToDictionary_ReturnsCorrectDictionary()
```

### Organización

1. **Arrange**: Preparar datos y dependencias
2. **Act**: Ejecutar el código bajo prueba
3. **Assert**: Verificar resultados

```csharp
[Fact]
public void Example_Test()
{
    // Arrange
    var config = new TecnoFactConfig("key", "secret", TecnoFactEnvironment.Sandbox);
    
    // Act
    var result = config.GetBaseUrl();
    
    // Assert
    Assert.Equal("https://sandbox.tecnofact.com/api", result);
}
```

### Tests Independientes

- Cada test debe ser independiente
- No compartir estado entre tests
- Usar `using` para recursos que implementan `IDisposable`

```csharp
[Fact]
public void Test_With_Disposable()
{
    using var httpClient = new TecnoFactHttpClient(_config);
    // Test code
}
```

### Tests Descriptivos

```csharp
// ❌ Mal
[Fact]
public void Test1() { }

// ✅ Bien
[Fact]
public void Constructor_WithNegativeTimeout_ThrowsArgumentException() { }
```

### Usar Theory para Múltiples Casos

```csharp
[Theory]
[InlineData(1)]
[InlineData(30)]
[InlineData(60)]
public void Config_WithDifferentTimeouts_IsValid(int timeout)
{
    var config = new TecnoFactConfig("key", "secret", 
        TecnoFactEnvironment.Sandbox, timeout: timeout);
    
    Assert.NotNull(config);
}
```

## 🔄 CI/CD

### GitHub Actions

Los tests se ejecutan automáticamente en:

- ✅ Push a `main` o `develop`
- ✅ Pull Requests
- ✅ Múltiples OS (Ubuntu, Windows, macOS)

```yaml
# .github/workflows/ci.yml
- name: Run tests
  run: dotnet test --configuration Release --no-build --verbosity normal
```

### Pre-Commit Hooks

```bash
# Ejecutar antes de commit
make pre-commit

# Esto ejecuta:
# 1. dotnet format (formateo)
# 2. dotnet build (compilación)
# 3. dotnet test (tests)
```

### Pipeline Completo

```bash
# Ejecutar pipeline completo localmente
make ci-full

# Incluye:
# - Build
# - Tests
# - Cobertura
# - Análisis de código
```

## 📊 Métricas de Calidad

### Objetivos

| Métrica | Objetivo | Actual |
|---------|----------|--------|
| Cobertura de Código | ≥ 90% | ~85% |
| Tests Pasando | 100% | 100% ✅ |
| Tiempo de Ejecución | < 30s | ~22s ✅ |
| Tests por Componente | ≥ 5 | ✅ |

### Monitoreo

- **Codecov**: Cobertura de código en cada PR
- **GitHub Actions**: Estado de build y tests
- **CodeQL**: Análisis de seguridad

## 🐛 Debugging Tests

### Tests Fallando

```bash
# Ver logs detallados
dotnet test --logger "console;verbosity=detailed"

# Ejecutar test específico
dotnet test --filter "FullyQualifiedName=TecnoFact.SDK.Tests.ConfigTests.Constructor_WithValidParameters_CreatesConfig"

# Debug en Visual Studio
# F5 en el archivo de test
```

### Tests Lentos

```bash
# Identificar tests lentos
dotnet test --logger "console;verbosity=detailed" | grep "ms]"

# Ejecutar en paralelo (por defecto)
dotnet test --parallel
```

### Problemas de Cobertura

```bash
# Limpiar y regenerar
dotnet clean
rm -rf coverage coverage-report
make docker-coverage
```

## 📚 Recursos

### Documentación

- [xUnit Documentation](https://xunit.net/)
- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

### Herramientas

- **xUnit**: Framework de testing
- **Coverlet**: Cobertura de código
- **Moq**: Mocking framework
- **FluentAssertions**: Assertions más legibles

## 🆘 Troubleshooting

### Tests No Se Ejecutan

```bash
# Restaurar dependencias
dotnet restore

# Limpiar y rebuild
dotnet clean
dotnet build
```

### Cobertura No Se Genera

```bash
# Verificar que coverlet esté instalado
dotnet list package | grep coverlet

# Reinstalar
dotnet add package coverlet.collector
dotnet add package coverlet.msbuild
```

### Docker Tests Fallan

```bash
# Reconstruir imagen
docker-compose build --no-cache sdk-tests

# Ver logs
docker-compose logs sdk-tests
```

---

## 📝 Agregar Nuevos Tests

### Template de Test

```csharp
using Xunit;
using TecnoFact.SDK.Models;

namespace TecnoFact.SDK.Tests;

public class NewFeatureTests
{
    [Fact]
    public void MethodName_Scenario_ExpectedBehavior()
    {
        // Arrange
        var input = "test";
        
        // Act
        var result = SomeMethod(input);
        
        // Assert
        Assert.Equal("expected", result);
    }
    
    [Theory]
    [InlineData("input1", "output1")]
    [InlineData("input2", "output2")]
    public void MethodName_WithDifferentInputs_ReturnsExpected(
        string input, string expected)
    {
        var result = SomeMethod(input);
        Assert.Equal(expected, result);
    }
}
```

### Checklist para Nuevos Tests

- [ ] Nombre descriptivo siguiendo convención
- [ ] Arrange-Act-Assert bien definido
- [ ] Test independiente (no depende de otros)
- [ ] Usa `using` para recursos desechables
- [ ] Incluye casos edge
- [ ] Documentado si es complejo

---

**¿Preguntas sobre testing?** Contacta a soporte@tecnofact.com
