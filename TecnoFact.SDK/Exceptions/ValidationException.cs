namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando hay un error de validación
/// </summary>
public class ValidationException : TecnoFactException
{
    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public ValidationException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
