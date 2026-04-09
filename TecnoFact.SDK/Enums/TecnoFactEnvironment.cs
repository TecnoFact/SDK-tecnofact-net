namespace TecnoFact.SDK.Enums;

/// <summary>
/// Entornos disponibles para el SDK de TecnoFact
/// </summary>
public enum TecnoFactEnvironment
{
    /// <summary>
    /// Entorno de pruebas (Sandbox)
    /// </summary>
    Sandbox,
    
    /// <summary>
    /// Entorno de producción
    /// </summary>
    Production
}

/// <summary>
/// Métodos de extensión para el enum TecnoFactEnvironment
/// </summary>
public static class TecnoFactEnvironmentExtensions
{
    /// <summary>
    /// Verifica si el entorno es producción
    /// </summary>
    public static bool IsProduction(this TecnoFactEnvironment environment)
        => environment == TecnoFactEnvironment.Production;

    /// <summary>
    /// Verifica si el entorno es sandbox
    /// </summary>
    public static bool IsSandbox(this TecnoFactEnvironment environment)
        => environment == TecnoFactEnvironment.Sandbox;

    /// <summary>
    /// Obtiene la etiqueta del entorno
    /// </summary>
    public static string Label(this TecnoFactEnvironment environment)
        => environment switch
        {
            TecnoFactEnvironment.Sandbox => "Sandbox",
            TecnoFactEnvironment.Production => "Producción",
            _ => throw new ArgumentOutOfRangeException(nameof(environment))
        };

    /// <summary>
    /// Obtiene el valor string del entorno
    /// </summary>
    public static string Value(this TecnoFactEnvironment environment)
        => environment switch
        {
            TecnoFactEnvironment.Sandbox => "sandbox",
            TecnoFactEnvironment.Production => "production",
            _ => throw new ArgumentOutOfRangeException(nameof(environment))
        };

    /// <summary>
    /// Obtiene la URL base del entorno
    /// </summary>
    public static string GetBaseUrl(this TecnoFactEnvironment environment)
        => environment switch
        {
            TecnoFactEnvironment.Sandbox => "https://sandbox.tecnofact.com/api",
            TecnoFactEnvironment.Production => "https://api.tecnofact.com/api",
            _ => throw new ArgumentOutOfRangeException(nameof(environment))
        };
}
