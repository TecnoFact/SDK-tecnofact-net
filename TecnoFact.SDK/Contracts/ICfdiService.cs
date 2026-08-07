using TecnoFact.SDK.Models;

namespace TecnoFact.SDK.Contracts;

/// <summary>
/// Defines CFDI stamping operations.
/// </summary>
public interface ICfdiService
{
    /// <summary>
    /// Sends an XML CFDI for stamping.
    /// </summary>
    Task<ResultadoTimbrado> TimbrarXmlAsync(
        TimbrarXmlRequest request,
        CancellationToken cancellationToken = default);
}
