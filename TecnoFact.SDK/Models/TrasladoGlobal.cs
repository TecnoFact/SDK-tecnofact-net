using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Traslado global de impuestos
/// </summary>
public class TrasladoGlobal
{
    /// <summary>
    /// Base del impuesto
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

    /// <summary>
    /// Initializes a global tax transfer with its required base amount.
    /// </summary>
    public TrasladoGlobal(decimal baseImporte, string impuesto, string tipoFactor, decimal? tasaOCuota = null, decimal? importe = null)
    {
        Base = baseImporte;
        Impuesto = impuesto;
        TipoFactor = tipoFactor;
        TasaOCuota = tasaOCuota;
        Importe = importe;
    }

    /// <summary>
    /// Initializes a global tax transfer without a base amount.
    /// </summary>
    public TrasladoGlobal(string impuesto, string tipoFactor, decimal? tasaOCuota = null, decimal? importe = null)
        : this(0m, impuesto, tipoFactor, tasaOCuota, importe)
    {
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
