using TecnoFact.SDK.Contracts;
using TecnoFact.SDK.Exceptions;
using TecnoFact.SDK.Models;

namespace TecnoFact.SDK.Services;

/// <summary>
/// Provides CFDI stamping operations.
/// </summary>
public sealed class CfdiService : ICfdiService
{
    private const string StampCfdiEndpoint = "/api/v1/stamp-cfdi";
    private readonly IHttpClient _httpClient;

    /// <summary>
    /// Creates the CFDI service with the configured HTTP transport.
    /// </summary>
    public CfdiService(IHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc />
    public async Task<ResultadoTimbrado> TimbrarXmlAsync(
        TimbrarXmlRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            return await _httpClient.PostAsync<TimbrarXmlRequest, ResultadoTimbrado>(
                StampCfdiEndpoint,
                request,
                cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new TimbradoException("Failed to stamp XML.");
        }
    }
}
