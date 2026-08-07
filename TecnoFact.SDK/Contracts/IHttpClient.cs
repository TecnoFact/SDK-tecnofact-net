namespace TecnoFact.SDK.Contracts;

/// <summary>
/// Interfaz para el cliente HTTP
/// </summary>
public interface IHttpClient
{
    /// <summary>
    /// Autentica las credenciales de usuario del panel y configura la autorización Bearer.
    /// </summary>
    Task LoginAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza una petición GET
    /// </summary>
    Task<TResponse> GetAsync<TResponse>(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza una petición POST
    /// </summary>
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza una petición PUT
    /// </summary>
    Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza una petición DELETE
    /// </summary>
    Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza una petición PATCH
    /// </summary>
    Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);
}
