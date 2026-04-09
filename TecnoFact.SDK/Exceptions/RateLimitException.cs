namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando se excede el límite de peticiones
/// </summary>
public class RateLimitException : TecnoFactException
{
    public RateLimitException(string message) : base(message)
    {
    }

    public RateLimitException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public RateLimitException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
