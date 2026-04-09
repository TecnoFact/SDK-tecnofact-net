namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción base para todas las excepciones del SDK de TecnoFact
/// </summary>
public class TecnoFactException : Exception
{
    /// <summary>
    /// Detalles adicionales del error
    /// </summary>
    public Dictionary<string, object>? Details { get; }

    /// <summary>
    /// Código de estado HTTP si aplica
    /// </summary>
    public int? StatusCode { get; }

    public TecnoFactException(string message) : base(message)
    {
    }

    public TecnoFactException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public TecnoFactException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message)
    {
        Details = details;
        StatusCode = statusCode;
    }

    public TecnoFactException(string message, Exception innerException, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, innerException)
    {
        Details = details;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Obtiene los detalles del error
    /// </summary>
    public Dictionary<string, object> GetDetails() => Details ?? new Dictionary<string, object>();
}
