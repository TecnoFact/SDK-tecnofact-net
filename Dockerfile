# Dockerfile para SDK TecnoFact .NET
# Multi-stage build para optimizar tamaño de imagen

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["TecnoFact.SDK/TecnoFact.SDK.csproj", "TecnoFact.SDK/"]
COPY ["TecnoFact.SDK.Tests/TecnoFact.SDK.Tests.csproj", "TecnoFact.SDK.Tests/"]

# Restaurar dependencias
RUN dotnet restore "TecnoFact.SDK/TecnoFact.SDK.csproj"
RUN dotnet restore "TecnoFact.SDK.Tests/TecnoFact.SDK.Tests.csproj"

# Copiar el resto del código
COPY . .

# Build del proyecto
WORKDIR "/src/TecnoFact.SDK"
RUN dotnet build "TecnoFact.SDK.csproj" -c Release -o /app/build

# Stage 2: Test
FROM build AS test
WORKDIR /src/TecnoFact.SDK.Tests

# Instalar herramientas de cobertura
RUN dotnet tool install --global dotnet-coverage
ENV PATH="${PATH}:/root/.dotnet/tools"

# Ejecutar tests con cobertura
RUN dotnet test "TecnoFact.SDK.Tests.csproj" \
    --configuration Release \
    --logger "trx;LogFileName=test_results.trx" \
    --collect:"XPlat Code Coverage" \
    --results-directory /testresults

# Stage 3: Publish
FROM build AS publish
WORKDIR "/src/TecnoFact.SDK"
RUN dotnet publish "TecnoFact.SDK.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Runtime (para ejemplos o uso del SDK)
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Metadata
LABEL maintainer="TecnoFact <soporte@tecnofact.com>"
LABEL description="TecnoFact SDK para .NET - Facturación Electrónica CFDI 4.0"
LABEL version="1.0.0"

# Stage final para desarrollo
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS development
WORKDIR /workspace

# Instalar herramientas útiles
RUN dotnet tool install --global dotnet-format
RUN dotnet tool install --global dotnet-coverage
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copiar código fuente
COPY . .

# Comando por defecto
CMD ["dotnet", "test", "--logger", "console;verbosity=detailed"]
