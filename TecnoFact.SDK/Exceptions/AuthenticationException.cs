namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando hay un error de autenticación
/// </summary>
public class AuthenticationException : TecnoFactException
{
    public AuthenticationException(string message) : base(message)
    {
    }

    public AuthenticationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public AuthenticationException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
