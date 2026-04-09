using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Retención global de impuestos
/// </summary>
public class RetencionGlobal
{
    /// <summary>
    /// Clave del impuesto
    /// </summary>
    [JsonPropertyName("impuesto")]
    public string Impuesto { get; set; } = string.Empty;

    /// <summary>
    /// Importe de la retención
    /// </summary>
    [JsonPropertyName("importe")]
    public decimal Importe { get; set; }

    public RetencionGlobal()
    {
    }

    public RetencionGlobal(string impuesto, decimal importe)
    {
        Impuesto = impuesto;
        Importe = importe;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["impuesto"] = Impuesto,
            ["importe"] = Importe
        };
    }
}
