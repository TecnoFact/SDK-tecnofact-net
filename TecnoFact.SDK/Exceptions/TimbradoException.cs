namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando hay un error en el proceso de timbrado
/// </summary>
public class TimbradoException : TecnoFactException
{
    public TimbradoException(string message) : base(message)
    {
    }

    public TimbradoException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public TimbradoException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
