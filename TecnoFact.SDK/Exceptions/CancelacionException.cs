namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando hay un error en el proceso de cancelación
/// </summary>
public class CancelacionException : TecnoFactException
{
    public CancelacionException(string message) : base(message)
    {
    }

    public CancelacionException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public CancelacionException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
