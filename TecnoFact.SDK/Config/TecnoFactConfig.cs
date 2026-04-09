using TecnoFact.SDK.Enums;

namespace TecnoFact.SDK.Config;

/// <summary>
/// Configuración inmutable del SDK de TecnoFact
/// </summary>
public class TecnoFactConfig
{
    /// <summary>
    /// API Key de TecnoFact
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    /// API Secret de TecnoFact
    /// </summary>
    public string ApiSecret { get; }

    /// <summary>
    /// Entorno de ejecución
    /// </summary>
    public TecnoFactEnvironment Environment { get; }

    /// <summary>
    /// Timeout en segundos para las peticiones HTTP
    /// </summary>
    public int Timeout { get; }

    /// <summary>
    /// Número de reintentos en caso de error
    /// </summary>
    public int Retries { get; }

    /// <summary>
    /// Constructor de la configuración
    /// </summary>
    /// <param name="apiKey">API Key de TecnoFact</param>
    /// <param name="apiSecret">API Secret de TecnoFact</param>
    /// <param name="environment">Entorno (Sandbox o Production)</param>
    /// <param name="timeout">Timeout en segundos (por defecto 30)</param>
    /// <param name="retries">Número de reintentos (por defecto 3)</param>
    /// <exception cref="ArgumentException">Si los parámetros no son válidos</exception>
    public TecnoFactConfig(
        string apiKey,
        string apiSecret,
        TecnoFactEnvironment environment = TecnoFactEnvironment.Sandbox,
        int timeout = 30,
        int retries = 3)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("api_key is required", nameof(apiKey));

        if (string.IsNullOrWhiteSpace(apiSecret))
            throw new ArgumentException("api_secret is required", nameof(apiSecret));

        if (timeout <= 0)
            throw new ArgumentException("timeout must be greater than 0", nameof(timeout));

        if (retries < 0)
            throw new ArgumentException("retries must be non-negative", nameof(retries));

        ApiKey = apiKey;
        ApiSecret = apiSecret;
        Environment = environment;
        Timeout = timeout;
        Retries = retries;
    }

    /// <summary>
    /// Obtiene la URL base del entorno configurado
    /// </summary>
    public string GetBaseUrl() => Environment.GetBaseUrl();

    /// <summary>
    /// Obtiene el entorno configurado
    /// </summary>
    public TecnoFactEnvironment GetEnvironment() => Environment;

    /// <summary>
    /// Obtiene el timeout configurado
    /// </summary>
    public int GetTimeout() => Timeout;

    /// <summary>
    /// Obtiene el número de reintentos configurado
    /// </summary>
    public int GetRetries() => Retries;

    /// <summary>
    /// Convierte la configuración a un diccionario
    /// </summary>
    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["api_key"] = ApiKey,
            ["api_secret"] = ApiSecret,
            ["environment"] = Environment.Value(),
            ["base_url"] = GetBaseUrl(),
            ["timeout"] = Timeout,
            ["retries"] = Retries
        };
    }
}
