# Política de Seguridad

## Versiones Soportadas

Actualmente damos soporte de seguridad a las siguientes versiones del SDK:

| Versión | Soportada          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Reportar una Vulnerabilidad

La seguridad de nuestro SDK es una prioridad. Si descubres una vulnerabilidad de seguridad, por favor repórtala de manera responsable.

### Proceso de Reporte

1. **NO** abras un issue público para vulnerabilidades de seguridad
2. Envía un correo a: **security@tecnofact.com**
3. Incluye la siguiente información:
   - Descripción detallada de la vulnerabilidad
   - Pasos para reproducir el problema
   - Versiones afectadas
   - Impacto potencial
   - Solución propuesta (si la tienes)

### Qué Esperar

- **Confirmación**: Recibirás una confirmación en 48 horas
- **Evaluación**: Evaluaremos la vulnerabilidad en 5 días hábiles
- **Actualización**: Te mantendremos informado del progreso
- **Resolución**: Trabajaremos en un fix y coordinaremos la divulgación
- **Crédito**: Reconoceremos tu contribución (si lo deseas)

### Divulgación Responsable

Solicitamos que:
- Nos des tiempo razonable para resolver el problema antes de divulgarlo públicamente
- No explotes la vulnerabilidad
- No accedas o modifiques datos de otros usuarios

### Recompensas

Aunque no tenemos un programa formal de bug bounty, reconocemos y agradecemos las contribuciones de seguridad:
- Mención en el CHANGELOG
- Crédito en las notas de la versión
- Agradecimiento público (si lo deseas)

## Mejores Prácticas de Seguridad

### Para Usuarios del SDK

1. **Credenciales**:
   - Nunca hardcodees API keys en el código
   - Usa variables de entorno o gestores de secretos
   - Rota las credenciales regularmente

2. **Actualizaciones**:
   - Mantén el SDK actualizado a la última versión
   - Revisa el CHANGELOG para cambios de seguridad
   - Suscríbete a las notificaciones de releases

3. **Validación**:
   - Valida todos los datos de entrada
   - Usa las validaciones integradas del SDK
   - No confíes en datos no validados

4. **HTTPS**:
   - Siempre usa HTTPS en producción
   - Verifica certificados SSL/TLS
   - No deshabilites la validación de certificados

5. **Logs**:
   - No registres información sensible
   - Sanitiza los logs antes de almacenarlos
   - Protege los archivos de log

### Para Contribuidores

1. **Código**:
   - Sigue las guías de código seguro
   - Usa análisis estático (CodeQL está habilitado)
   - Revisa las dependencias regularmente

2. **Dependencies**:
   - Mantén las dependencias actualizadas
   - Revisa los advisories de seguridad
   - Usa Dependabot (habilitado automáticamente)

3. **Tests**:
   - Escribe tests de seguridad
   - Prueba casos límite
   - Valida el manejo de errores

## Análisis de Seguridad Automatizado

Este proyecto utiliza:

- ✅ **CodeQL**: Análisis de código estático para detectar vulnerabilidades
- ✅ **Dependabot**: Actualizaciones automáticas de dependencias
- ✅ **GitHub Security Advisories**: Alertas de vulnerabilidades conocidas
- ✅ **Automated Security Updates**: Parches automáticos cuando es posible

## Contacto

Para consultas de seguridad:
- Email: security@tecnofact.com
- Respuesta esperada: 48 horas

Para consultas generales:
- Email: soporte@tecnofact.com
- GitHub Issues: [Crear Issue](https://github.com/TecnoFact/SDK-tecnofact-net/issues)

## Agradecimientos

Agradecemos a todos los investigadores de seguridad que han contribuido a mejorar la seguridad de este SDK.

---

Última actualización: 10 de Abril de 2026
