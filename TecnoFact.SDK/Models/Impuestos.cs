using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Impuestos globales del CFDI
/// </summary>
public class Impuestos
{
    /// <summary>
    /// Total de impuestos retenidos
    /// </summary>
    [JsonPropertyName("total_impuestos_retenidos")]
    public decimal? TotalImpuestosRetenidos { get; set; }

    /// <summary>
    /// Total de impuestos trasladados
    /// </summary>
    [JsonPropertyName("total_impuestos_trasladados")]
    public decimal? TotalImpuestosTrasladados { get; set; }

    /// <summary>
    /// Lista de retenciones globales
    /// </summary>
    [JsonPropertyName("retenciones")]
    public List<RetencionGlobal>? Retenciones { get; set; }

    /// <summary>
    /// Lista de traslados globales
    /// </summary>
    [JsonPropertyName("traslados")]
    public List<TrasladoGlobal>? Traslados { get; set; }

    public Impuestos()
    {
    }

    public Impuestos(decimal? totalImpuestosRetenidos = null, decimal? totalImpuestosTrasladados = null,
                    List<RetencionGlobal>? retenciones = null, List<TrasladoGlobal>? traslados = null)
    {
        TotalImpuestosRetenidos = totalImpuestosRetenidos;
        TotalImpuestosTrasladados = totalImpuestosTrasladados;
        Retenciones = retenciones;
        Traslados = traslados;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["total_impuestos_retenidos"] = TotalImpuestosRetenidos,
            ["total_impuestos_trasladados"] = TotalImpuestosTrasladados,
            ["retenciones"] = Retenciones?.Select(r => r.ToDictionary()).ToList(),
            ["traslados"] = Traslados?.Select(t => t.ToDictionary()).ToList()
        };
    }
}
