# Guía de Contribución

¡Gracias por tu interés en contribuir al SDK de TecnoFact para .NET! Esta guía te ayudará a comenzar.

## 🚀 Comenzando

### Requisitos Previos

- .NET SDK 10.0 o superior
- Git
- Un IDE compatible (.NET, Visual Studio, VS Code con C# Dev Kit, Rider)

### Configuración del Entorno de Desarrollo

1. Fork el repositorio
2. Clona tu fork:
   ```bash
   git clone https://github.com/TU_USUARIO/SDK-tecnofact-net.git
   cd SDK-tecnofact-net
   ```

3. Restaura las dependencias:
   ```bash
   dotnet restore
   ```

4. Compila el proyecto:
   ```bash
   dotnet build
   ```

5. Ejecuta los tests:
   ```bash
   dotnet test
   ```

## 📝 Estándares de Código

### Convenciones de C#

- Seguir las [convenciones de código de C#](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Usar nullable reference types
- Escribir documentación XML para todos los miembros públicos
- Usar nombres descriptivos en inglés para variables y métodos internos
- Usar nombres en español para propiedades que mapean a la API de TecnoFact

### Formato de Código

```bash
# Formatear código automáticamente
dotnet format
```

### Análisis Estático

El proyecto está configurado para generar advertencias por comentarios XML faltantes. Asegúrate de documentar todos los miembros públicos.

## 🧪 Testing

### Escribir Tests

- Todos los nuevos features deben incluir tests
- Mantener cobertura de tests > 80%
- Usar nombres descriptivos para los tests: `Method_Scenario_ExpectedResult`
- Usar xUnit como framework de testing

### Ejecutar Tests

```bash
# Todos los tests
dotnet test

# Con cobertura
dotnet test /p:CollectCoverage=true

# Tests específicos
dotnet test --filter "FullyQualifiedName~ConfigTests"
```

## 🔄 Proceso de Contribución

1. **Crea una rama** para tu feature o fix:
   ```bash
   git checkout -b feature/nombre-descriptivo
   # o
   git checkout -b fix/descripcion-del-bug
   ```

2. **Realiza tus cambios** siguiendo los estándares de código

3. **Escribe tests** para tus cambios

4. **Ejecuta los tests** y asegúrate de que todos pasen:
   ```bash
   dotnet test
   ```

5. **Compila el proyecto** sin advertencias críticas:
   ```bash
   dotnet build
   ```

6. **Commit tus cambios** con mensajes descriptivos:
   ```bash
   git commit -m "feat: agregar soporte para CFDI 3.3"
   # o
   git commit -m "fix: corregir validación de RFC"
   ```

7. **Push a tu fork**:
   ```bash
   git push origin feature/nombre-descriptivo
   ```

8. **Abre un Pull Request** en el repositorio principal

## 📋 Tipos de Contribuciones

### 🐛 Reportar Bugs

- Usa el template de issue para bugs
- Incluye pasos para reproducir el problema
- Proporciona información del entorno (.NET version, OS, etc.)
- Incluye stack traces si están disponibles

### ✨ Proponer Features

- Usa el template de issue para features
- Describe el caso de uso
- Explica cómo beneficiaría a los usuarios
- Considera la compatibilidad con versiones anteriores

### 📖 Mejorar Documentación

- Corrige errores tipográficos
- Mejora ejemplos de código
- Agrega casos de uso adicionales
- Traduce documentación

## 🎯 Áreas de Enfoque

Áreas donde las contribuciones son especialmente bienvenidas:

- Ejemplos de uso adicionales
- Mejoras en el manejo de errores
- Optimizaciones de rendimiento
- Soporte para más features de CFDI
- Documentación y tutoriales
- Tests adicionales

## ⚖️ Licencia

Al contribuir, aceptas que tus contribuciones serán licenciadas bajo la Licencia MIT del proyecto.

## 💬 ¿Preguntas?

Si tienes preguntas sobre cómo contribuir:

- Abre un issue con la etiqueta `question`
- Contacta al equipo en soporte@tecnofact.com

¡Gracias por contribuir! 🎉
