using TecnoFact.SDK.Enums;
using System.Net.Mail;

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
    /// Email de la cuenta TecnoFact para la autenticación del panel
    /// </summary>
    public string? Email { get; }

    /// <summary>
    /// Contraseña de la cuenta TecnoFact para la autenticación del panel
    /// </summary>
    public string? Password { get; }

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

    private TecnoFactConfig(
        string email,
        string password,
        TecnoFactEnvironment environment,
        int timeout,
        int retries,
        bool usesUserCredentials)
    {
        ApiKey = string.Empty;
        ApiSecret = string.Empty;
        Email = email;
        Password = password;
        Environment = environment;
        Timeout = timeout;
        Retries = retries;
    }

    /// <summary>
    /// Crea una configuración con las credenciales de usuario del panel TecnoFact.
    /// </summary>
    /// <param name="email">Correo electrónico de la cuenta TecnoFact</param>
    /// <param name="password">Contraseña de la cuenta TecnoFact</param>
    /// <param name="environment">Entorno (Sandbox o Production)</param>
    /// <param name="timeout">Timeout en segundos (por defecto 30)</param>
    /// <param name="retries">Número de reintentos (por defecto 3)</param>
    /// <exception cref="ArgumentException">Si los parámetros no son válidos</exception>
    public static TecnoFactConfig ForUserCredentials(
        string email,
        string password,
        TecnoFactEnvironment environment = TecnoFactEnvironment.Sandbox,
        int timeout = 30,
        int retries = 3)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("email is required", nameof(email));

        try
        {
            if (!string.Equals(new MailAddress(email).Address, email, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("email must be valid", nameof(email));
        }
        catch (FormatException)
        {
            throw new ArgumentException("email must be valid", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("password is required", nameof(password));

        if (timeout <= 0)
            throw new ArgumentException("timeout must be greater than 0", nameof(timeout));

        if (retries < 0)
            throw new ArgumentException("retries must be non-negative", nameof(retries));

        return new TecnoFactConfig(email, password, environment, timeout, retries, usesUserCredentials: true);
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
        var values = new Dictionary<string, object>
        {
            ["environment"] = Environment.Value(),
            ["base_url"] = GetBaseUrl(),
            ["timeout"] = Timeout,
            ["retries"] = Retries
        };

        if (!string.IsNullOrEmpty(ApiKey))
        {
            values["api_key"] = ApiKey;
            values["api_secret"] = ApiSecret;
        }

        return values;
    }
}
