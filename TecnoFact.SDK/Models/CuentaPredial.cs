using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Cuenta predial
/// </summary>
public class CuentaPredial
{
    /// <summary>
    /// Número de cuenta predial
    /// </summary>
    [JsonPropertyName("numero")]
    public string Numero { get; set; } = string.Empty;

    public CuentaPredial()
    {
    }

    public CuentaPredial(string numero)
    {
        Numero = numero;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["numero"] = Numero
        };
    }
}
