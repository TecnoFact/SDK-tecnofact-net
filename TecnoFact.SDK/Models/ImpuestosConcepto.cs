using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Impuestos aplicables a un concepto
/// </summary>
public class ImpuestosConcepto
{
    /// <summary>
    /// Lista de traslados de impuestos
    /// </summary>
    [JsonPropertyName("traslados")]
    public List<Traslado>? Traslados { get; set; }

    /// <summary>
    /// Lista de retenciones de impuestos
    /// </summary>
    [JsonPropertyName("retenciones")]
    public List<Retencion>? Retenciones { get; set; }

    public ImpuestosConcepto()
    {
    }

    public ImpuestosConcepto(List<Traslado>? traslados = null, List<Retencion>? retenciones = null)
    {
        Traslados = traslados;
        Retenciones = retenciones;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["traslados"] = Traslados?.Select(t => t.ToDictionary()).ToList(),
            ["retenciones"] = Retenciones?.Select(r => r.ToDictionary()).ToList()
        };
    }
}
