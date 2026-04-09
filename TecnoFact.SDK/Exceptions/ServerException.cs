namespace TecnoFact.SDK.Exceptions;

/// <summary>
/// Excepción lanzada cuando hay un error del servidor
/// </summary>
public class ServerException : TecnoFactException
{
    public ServerException(string message) : base(message)
    {
    }

    public ServerException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public ServerException(string message, Dictionary<string, object>? details = null, int? statusCode = null)
        : base(message, details, statusCode)
    {
    }
}
