# 🐳 Guía de Docker para TecnoFact SDK

Esta guía explica cómo usar Docker para desarrollo, testing y CI/CD del SDK de TecnoFact.

## 📋 Tabla de Contenidos

- [Requisitos](#requisitos)
- [Inicio Rápido](#inicio-rápido)
- [Comandos Disponibles](#comandos-disponibles)
- [Ambientes](#ambientes)
- [Testing con Docker](#testing-con-docker)
- [Desarrollo con Docker](#desarrollo-con-docker)
- [CI/CD](#cicd)
- [Troubleshooting](#troubleshooting)

## 🔧 Requisitos

- Docker >= 20.10
- Docker Compose >= 2.0
- Make (opcional, para usar Makefile)

### Instalación de Docker

**Windows:**
```bash
# Descargar Docker Desktop desde https://www.docker.com/products/docker-desktop
```

**Linux:**
```bash
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
```

**macOS:**
```bash
brew install --cask docker
```

## 🚀 Inicio Rápido

### 1. Construir la Imagen

```bash
# Usando Make
make docker-build

# O directamente con Docker Compose
docker-compose build
```

### 2. Ejecutar Tests

```bash
# Usando Make
make docker-test

# O directamente con Docker Compose
docker-compose run --rm sdk-tests
```

### 3. Ver Resultados

Los resultados de tests se guardan en `./testresults/`

## 📝 Comandos Disponibles

### Usando Makefile (Recomendado)

```bash
# Ver todos los comandos disponibles
make help

# Comandos locales (sin Docker)
make restore          # Restaurar dependencias
make build           # Compilar proyecto
make test            # Ejecutar tests
make test-watch      # Tests en modo watch
make coverage        # Generar cobertura
make format          # Formatear código
make clean           # Limpiar archivos

# Comandos Docker
make docker-build         # Construir imagen
make docker-test          # Ejecutar tests en Docker
make docker-dev           # Ambiente de desarrollo
make docker-coverage      # Cobertura en Docker
make docker-integration   # Tests de integración
make docker-analysis      # Análisis de código
make docker-clean         # Limpiar Docker
make docker-shell         # Shell en contenedor

# Comandos CI/CD
make ci-test         # Tests para CI
make ci-coverage     # Cobertura para CI
make ci-full         # Pipeline completo

# Comandos de utilidad
make verify          # Verificar código completo
make pre-commit      # Checks antes de commit
make info            # Información del proyecto
```

### Usando Docker Compose Directamente

```bash
# Ejecutar tests
docker-compose run --rm sdk-tests

# Ambiente de desarrollo con hot reload
docker-compose up sdk-dev

# Generar reporte de cobertura
docker-compose run --rm coverage-report

# Tests de integración
docker-compose up -d mock-api
docker-compose run --rm sdk-integration-tests
docker-compose down

# Análisis de código
docker-compose run --rm code-analysis

# Shell interactivo
docker-compose run --rm sdk-dev /bin/bash
```

## 🏗️ Ambientes

### 1. Test (sdk-tests)

Ejecuta tests unitarios con cobertura de código.

```bash
docker-compose run --rm sdk-tests
```

**Características:**
- Ejecuta todos los tests unitarios
- Genera cobertura de código
- Guarda resultados en `/testresults`
- Imagen optimizada para CI/CD

### 2. Development (sdk-dev)

Ambiente de desarrollo con hot reload.

```bash
docker-compose up sdk-dev
```

**Características:**
- Hot reload automático
- Volumen montado para edición en tiempo real
- Herramientas de desarrollo instaladas
- Watch mode para tests

### 3. Coverage Report (coverage-report)

Genera reportes HTML de cobertura.

```bash
docker-compose run --rm coverage-report
```

**Características:**
- Genera reporte HTML
- Badges de cobertura
- Resumen en texto
- Salida en `/coverage-report`

### 4. Integration Tests (sdk-integration-tests)

Tests de integración con mock API.

```bash
docker-compose up -d mock-api
docker-compose run --rm sdk-integration-tests
```

**Características:**
- Mock API con MockServer
- Tests contra endpoints reales
- Variables de entorno configurables

## 🧪 Testing con Docker

### Tests Unitarios

```bash
# Ejecutar todos los tests
make docker-test

# Ver logs detallados
docker-compose run --rm sdk-tests dotnet test --logger "console;verbosity=detailed"

# Ejecutar tests específicos
docker-compose run --rm sdk-tests dotnet test --filter "FullyQualifiedName~ConfigTests"
```

### Tests con Cobertura

```bash
# Generar cobertura
make docker-coverage

# Ver reporte HTML
open coverage-report/index.html  # macOS
xdg-open coverage-report/index.html  # Linux
start coverage-report/index.html  # Windows
```

### Tests de Integración

```bash
# Ejecutar tests de integración
make docker-integration

# O manualmente
docker-compose up -d mock-api
docker-compose run --rm sdk-integration-tests
docker-compose down
```

### Tests en Modo Watch

```bash
# Iniciar watch mode
docker-compose up sdk-dev

# Los tests se ejecutarán automáticamente al guardar cambios
```

## 💻 Desarrollo con Docker

### Iniciar Ambiente de Desarrollo

```bash
# Iniciar contenedor de desarrollo
docker-compose up sdk-dev

# En otra terminal, puedes ejecutar comandos
docker-compose exec sdk-dev dotnet build
docker-compose exec sdk-dev dotnet test
```

### Editar Código

El código se monta como volumen, puedes editar archivos localmente y los cambios se reflejan en el contenedor.

```bash
# Estructura de volúmenes
volumes:
  - .:/workspace              # Todo el proyecto
  - dotnet-packages:/root/.nuget/packages  # Cache de NuGet
```

### Formatear Código

```bash
# Formatear código en Docker
docker-compose run --rm sdk-dev dotnet format

# O usando Make
make format
```

### Shell Interactivo

```bash
# Abrir bash en el contenedor
make docker-shell

# O directamente
docker-compose run --rm sdk-dev /bin/bash

# Dentro del contenedor puedes ejecutar:
dotnet build
dotnet test
dotnet format
```

## 🔄 CI/CD

### GitHub Actions

Los workflows de GitHub Actions usan Docker automáticamente:

```yaml
# .github/workflows/ci.yml
- name: Run tests in Docker
  run: docker-compose run --rm sdk-tests
```

### Pipeline Completo

```bash
# Ejecutar pipeline completo localmente
make ci-full

# Esto ejecuta:
# 1. docker-build
# 2. docker-test
# 3. docker-coverage
# 4. docker-analysis
```

### Verificación Pre-Commit

```bash
# Ejecutar antes de hacer commit
make pre-commit

# Esto ejecuta:
# 1. format (formateo de código)
# 2. build (compilación)
# 3. test (tests unitarios)
```

## 🐛 Troubleshooting

### Error: "Cannot connect to Docker daemon"

```bash
# Verificar que Docker esté corriendo
docker ps

# Iniciar Docker Desktop (Windows/Mac)
# O iniciar servicio (Linux)
sudo systemctl start docker
```

### Error: "Port already in use"

```bash
# Detener contenedores existentes
docker-compose down

# Limpiar todo
make docker-clean
```

### Tests Fallan en Docker pero Pasan Localmente

```bash
# Reconstruir imagen sin cache
docker-compose build --no-cache

# Limpiar volúmenes
docker-compose down -v
```

### Problemas de Permisos (Linux)

```bash
# Agregar usuario al grupo docker
sudo usermod -aG docker $USER

# Logout y login nuevamente
```

### Imagen Muy Grande

```bash
# Limpiar imágenes no usadas
docker system prune -a

# Ver tamaño de imágenes
docker images | grep tecnofact
```

### Hot Reload No Funciona

```bash
# Asegurar que DOTNET_USE_POLLING_FILE_WATCHER está configurado
docker-compose up sdk-dev

# Verificar logs
docker-compose logs -f sdk-dev
```

## 📊 Métricas y Reportes

### Cobertura de Código

```bash
# Generar reporte de cobertura
make docker-coverage

# Archivos generados:
# - coverage-report/index.html (reporte HTML)
# - coverage-report/badge_combined.svg (badge)
# - coverage-report/Summary.txt (resumen)
```

### Análisis de Código

```bash
# Ejecutar análisis
make docker-analysis

# Verifica:
# - Formato de código (dotnet format)
# - Warnings como errores
# - Estándares de código
```

## 🔐 Seguridad

### Variables de Entorno

```bash
# Crear archivo .env para variables sensibles
cp .env.example .env

# Editar .env con tus credenciales
TECNOFACT_API_KEY=tu_api_key
TECNOFACT_API_SECRET=tu_api_secret
```

### Secrets en CI/CD

No incluyas credenciales en el código. Usa GitHub Secrets:

```bash
# En GitHub: Settings > Secrets > Actions
NUGET_API_KEY
CODECOV_TOKEN
TECNOFACT_API_KEY
TECNOFACT_API_SECRET
```

## 📚 Recursos Adicionales

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [Dockerfile Best Practices](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/)

## 🆘 Soporte

Si encuentras problemas:

1. Revisa esta guía de troubleshooting
2. Busca en [GitHub Issues](https://github.com/TecnoFact/SDK-tecnofact-net/issues)
3. Crea un nuevo issue con:
   - Versión de Docker
   - Sistema operativo
   - Comando ejecutado
   - Error completo

---

**¿Preguntas?** Contacta a soporte@tecnofact.com
