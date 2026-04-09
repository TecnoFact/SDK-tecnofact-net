using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Información aduanera
/// </summary>
public class InformacionAduanera
{
    /// <summary>
    /// Número de pedimento aduanal
    /// </summary>
    [JsonPropertyName("numero_pedimento")]
    public string NumeroPedimento { get; set; } = string.Empty;

    public InformacionAduanera()
    {
    }

    public InformacionAduanera(string numeroPedimento)
    {
        NumeroPedimento = numeroPedimento;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["numero_pedimento"] = NumeroPedimento
        };
    }
}
