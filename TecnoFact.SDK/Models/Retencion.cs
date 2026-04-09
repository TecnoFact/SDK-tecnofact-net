using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Retención de impuestos
/// </summary>
public class Retencion
{
    /// <summary>
    /// Base de la retención
    /// </summary>
    [JsonPropertyName("base")]
    public decimal Base { get; set; }

    /// <summary>
    /// Clave del impuesto
    /// </summary>
    [JsonPropertyName("impuesto")]
    public string Impuesto { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de factor
    /// </summary>
    [JsonPropertyName("tipo_factor")]
    public string TipoFactor { get; set; } = string.Empty;

    /// <summary>
    /// Tasa o cuota de la retención
    /// </summary>
    [JsonPropertyName("tasa_o_cuota")]
    public decimal TasaOCuota { get; set; }

    /// <summary>
    /// Importe de la retención
    /// </summary>
    [JsonPropertyName("importe")]
    public decimal Importe { get; set; }

    public Retencion()
    {
    }

    public Retencion(decimal baseImporte, string impuesto, string tipoFactor, decimal tasaOCuota, decimal importe)
    {
        Base = baseImporte;
        Impuesto = impuesto;
        TipoFactor = tipoFactor;
        TasaOCuota = tasaOCuota;
        Importe = importe;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["base"] = Base,
            ["impuesto"] = Impuesto,
            ["tipo_factor"] = TipoFactor,
            ["tasa_o_cuota"] = TasaOCuota,
            ["importe"] = Importe
        };
    }
}
