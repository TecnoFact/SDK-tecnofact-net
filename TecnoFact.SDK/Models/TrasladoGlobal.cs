using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Traslado global de impuestos
/// </summary>
public class TrasladoGlobal
{
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
    /// Tasa o cuota
    /// </summary>
    [JsonPropertyName("tasa_o_cuota")]
    public decimal? TasaOCuota { get; set; }

    /// <summary>
    /// Importe del traslado
    /// </summary>
    [JsonPropertyName("importe")]
    public decimal? Importe { get; set; }

    public TrasladoGlobal()
    {
    }

    public TrasladoGlobal(string impuesto, string tipoFactor, decimal? tasaOCuota = null, decimal? importe = null)
    {
        Impuesto = impuesto;
        TipoFactor = tipoFactor;
        TasaOCuota = tasaOCuota;
        Importe = importe;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["impuesto"] = Impuesto,
            ["tipo_factor"] = TipoFactor,
            ["tasa_o_cuota"] = TasaOCuota,
            ["importe"] = Importe
        };
    }
}
