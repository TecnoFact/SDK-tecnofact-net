using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Traslado de impuestos
/// </summary>
public class Traslado
{
    /// <summary>
    /// Base del impuesto
    /// </summary>
    [JsonPropertyName("base")]
    public decimal Base { get; set; }

    /// <summary>
    /// Clave del impuesto (002 para IVA, 003 para IEPS)
    /// </summary>
    [JsonPropertyName("impuesto")]
    public string Impuesto { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de factor (Tasa, Cuota, Exento)
    /// </summary>
    [JsonPropertyName("tipo_factor")]
    public string TipoFactor { get; set; } = string.Empty;

    /// <summary>
    /// Tasa o cuota del impuesto
    /// </summary>
    [JsonPropertyName("tasa_o_cuota")]
    public decimal? TasaOCuota { get; set; }

    /// <summary>
    /// Importe del impuesto trasladado
    /// </summary>
    [JsonPropertyName("importe")]
    public decimal? Importe { get; set; }

    public Traslado()
    {
    }

    public Traslado(decimal baseImporte, string impuesto, string tipoFactor, decimal? tasaOCuota = null, decimal? importe = null)
    {
        Base = baseImporte;
        Impuesto = impuesto;
        TipoFactor = tipoFactor;
        TasaOCuota = tasaOCuota;
        Importe = importe;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["base"] = Base,
            ["impuesto"] = Impuesto,
            ["tipo_factor"] = TipoFactor,
            ["tasa_o_cuota"] = TasaOCuota,
            ["importe"] = Importe
        };
    }
}
