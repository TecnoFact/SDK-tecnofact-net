namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando no se encuentra un recurso
/// </summary>
public class NotFoundException : TecnoFactException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public NotFoundException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
