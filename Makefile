.PHONY: help build test test-watch coverage clean docker-build docker-test docker-dev docker-coverage docker-clean format restore

# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
PROJECT_NAME = TecnoFact.SDK
TEST_PROJECT = TecnoFact.SDK.Tests

# Colores para output
GREEN = \033[0;32m
YELLOW = \033[0;33m
RED = \033[0;31m
NC = \033[0m # No Color

help: ## Mostrar esta ayuda
	@echo "$(GREEN)Comandos disponibles:$(NC)"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  $(YELLOW)%-20s$(NC) %s\n", $$1, $$2}'

# Comandos locales (sin Docker)
restore: ## Restaurar dependencias de NuGet
	@echo "$(GREEN)Restaurando dependencias...$(NC)"
	$(DOTNET) restore

build: restore ## Compilar el proyecto
	@echo "$(GREEN)Compilando proyecto...$(NC)"
	$(DOTNET) build --configuration Release --no-restore

test: ## Ejecutar tests unitarios
	@echo "$(GREEN)Ejecutando tests...$(NC)"
	$(DOTNET) test $(TEST_PROJECT)/$(TEST_PROJECT).csproj --configuration Release --logger "console;verbosity=detailed"

test-watch: ## Ejecutar tests en modo watch
	@echo "$(GREEN)Ejecutando tests en modo watch...$(NC)"
	$(DOTNET) watch test --project $(TEST_PROJECT)/$(TEST_PROJECT).csproj

coverage: ## Generar reporte de cobertura
	@echo "$(GREEN)Generando reporte de cobertura...$(NC)"
	$(DOTNET) test $(TEST_PROJECT)/$(TEST_PROJECT).csproj \
		--collect:"XPlat Code Coverage" \
		--results-directory ./coverage
	@echo "$(GREEN)Cobertura generada en ./coverage$(NC)"

format: ## Formatear código
	@echo "$(GREEN)Formateando código...$(NC)"
	$(DOTNET) format

clean: ## Limpiar archivos de build
	@echo "$(GREEN)Limpiando archivos de build...$(NC)"
	$(DOTNET) clean
	rm -rf **/bin **/obj
	rm -rf coverage coverage-report testresults

# Comandos Docker
docker-build: ## Construir imagen Docker
	@echo "$(GREEN)Construyendo imagen Docker...$(NC)"
	$(DOCKER_COMPOSE) build

docker-test: ## Ejecutar tests en Docker
	@echo "$(GREEN)Ejecutando tests en Docker...$(NC)"
	$(DOCKER_COMPOSE) run --rm sdk-tests

docker-dev: ## Iniciar ambiente de desarrollo en Docker
	@echo "$(GREEN)Iniciando ambiente de desarrollo...$(NC)"
	$(DOCKER_COMPOSE) up sdk-dev

docker-coverage: ## Generar reporte de cobertura en Docker
	@echo "$(GREEN)Generando reporte de cobertura en Docker...$(NC)"
	$(DOCKER_COMPOSE) run --rm coverage-report
	@echo "$(GREEN)Reporte generado en ./coverage-report/index.html$(NC)"

docker-integration: ## Ejecutar tests de integración en Docker
	@echo "$(GREEN)Ejecutando tests de integración...$(NC)"
	$(DOCKER_COMPOSE) up -d mock-api
	@sleep 5
	$(DOCKER_COMPOSE) run --rm sdk-integration-tests
	$(DOCKER_COMPOSE) down

docker-analysis: ## Ejecutar análisis de código en Docker
	@echo "$(GREEN)Ejecutando análisis de código...$(NC)"
	$(DOCKER_COMPOSE) run --rm code-analysis

docker-clean: ## Limpiar contenedores y volúmenes Docker
	@echo "$(GREEN)Limpiando Docker...$(NC)"
	$(DOCKER_COMPOSE) down -v
	docker system prune -f

docker-shell: ## Abrir shell en contenedor de desarrollo
	@echo "$(GREEN)Abriendo shell en contenedor...$(NC)"
	$(DOCKER_COMPOSE) run --rm sdk-dev /bin/bash

# Comandos CI/CD
ci-test: docker-build docker-test ## Ejecutar tests para CI
	@echo "$(GREEN)Tests CI completados$(NC)"

ci-coverage: docker-build docker-coverage ## Generar cobertura para CI
	@echo "$(GREEN)Cobertura CI completada$(NC)"

ci-full: docker-build docker-test docker-coverage docker-analysis ## Pipeline CI completo
	@echo "$(GREEN)Pipeline CI completo$(NC)"

# Comandos de release
pack: build ## Crear paquete NuGet
	@echo "$(GREEN)Creando paquete NuGet...$(NC)"
	$(DOTNET) pack $(PROJECT_NAME)/$(PROJECT_NAME).csproj \
		--configuration Release \
		--output ./artifacts

publish-local: pack ## Publicar paquete localmente
	@echo "$(GREEN)Publicando paquete localmente...$(NC)"
	$(DOTNET) nuget push ./artifacts/*.nupkg \
		--source ~/.nuget/local

# Comandos de utilidad
watch-tests: ## Ver tests en tiempo real
	@echo "$(GREEN)Observando tests...$(NC)"
	$(DOTNET) watch test --project $(TEST_PROJECT)/$(TEST_PROJECT).csproj

list-tests: ## Listar todos los tests
	@echo "$(GREEN)Listando tests...$(NC)"
	$(DOTNET) test $(TEST_PROJECT)/$(TEST_PROJECT).csproj --list-tests

run-example: ## Ejecutar ejemplo de uso
	@echo "$(GREEN)Ejecutando ejemplo...$(NC)"
	$(DOTNET) run --project Examples/BasicUsageExample.cs

# Comandos de verificación
verify: format build test ## Verificar código (format + build + test)
	@echo "$(GREEN)Verificación completada$(NC)"

pre-commit: verify ## Ejecutar antes de commit
	@echo "$(GREEN)Pre-commit checks completados$(NC)"

# Información del proyecto
info: ## Mostrar información del proyecto
	@echo "$(GREEN)=== Información del Proyecto ===$(NC)"
	@echo "Proyecto: $(PROJECT_NAME)"
	@echo "Tests: $(TEST_PROJECT)"
	@echo ".NET SDK: $$($(DOTNET) --version)"
	@echo "Docker: $$(docker --version 2>/dev/null || echo 'No instalado')"
	@echo "Docker Compose: $$(docker-compose --version 2>/dev/null || echo 'No instalado')"
