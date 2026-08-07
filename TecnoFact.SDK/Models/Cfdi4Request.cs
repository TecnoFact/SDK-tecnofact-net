using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Solicitud para timbrar un CFDI 4.0
/// </summary>
public class Cfdi4Request
{
    /// <summary>
    /// Datos del emisor
    /// </summary>
    [JsonPropertyName("emisor")]
    public Emisor Emisor { get; set; } = new();

    /// <summary>
    /// Datos del receptor
    /// </summary>
    [JsonPropertyName("receptor")]
    public Receptor Receptor { get; set; } = new();

    /// <summary>
    /// Lista de conceptos
    /// </summary>
    [JsonPropertyName("conceptos")]
    public List<Concepto> Conceptos { get; set; } = new();

    /// <summary>
    /// Tipo de comprobante (I, E, T, N, P)
    /// </summary>
    [JsonPropertyName("tipo_comprobante")]
    public string? TipoComprobante { get; set; }

    /// <summary>
    /// Forma de pago
    /// </summary>
    [JsonPropertyName("forma_pago")]
    public string? FormaPago { get; set; }

    /// <summary>
    /// Método de pago (PUE, PPD)
    /// </summary>
    [JsonPropertyName("metodo_pago")]
    public string? MetodoPago { get; set; }

    /// <summary>
    /// Moneda
    /// </summary>
    [JsonPropertyName("moneda")]
    public string? Moneda { get; set; }

    /// <summary>
    /// Tipo de cambio
    /// </summary>
    [JsonPropertyName("tipo_cambio")]
    public decimal? TipoCambio { get; set; }

    /// <summary>
    /// Condiciones de pago
    /// </summary>
    [JsonPropertyName("condiciones_pago")]
    public string? CondicionesPago { get; set; }

    /// <summary>
    /// Subtotal
    /// </summary>
    [JsonPropertyName("subtotal")]
    public decimal? Subtotal { get; set; }

    /// <summary>
    /// Descuento
    /// </summary>
    [JsonPropertyName("descuento")]
    public decimal? Descuento { get; set; }

    /// <summary>
    /// Total
    /// </summary>
    [JsonPropertyName("total")]
    public decimal? Total { get; set; }

    /// <summary>
    /// Impuestos globales
    /// </summary>
    [JsonPropertyName("impuestos")]
    public Impuestos? Impuestos { get; set; }

    /// <summary>
    /// CFDIs relacionados
    /// </summary>
    [JsonPropertyName("cfdi_relacionados")]
    public List<CfdiRelacionados>? CfdiRelacionados { get; set; }

    /// <summary>
    /// Global invoice information for public-general operations
    /// </summary>
    [JsonPropertyName("informacion_global")]
    public InformacionGlobal? InformacionGlobal { get; set; }

    /// <summary>
    /// Exportación
    /// </summary>
    [JsonPropertyName("exportacion")]
    public string? Exportacion { get; set; }

    /// <summary>
    /// Fecha y hora de expedición
    /// </summary>
    [JsonPropertyName("fecha")]
    public DateTime? Fecha { get; set; }

    /// <summary>
    /// Código postal del lugar de expedición
    /// </summary>
    [JsonPropertyName("lugar_expedicion")]
    public string? LugarExpedicion { get; set; }

    public Cfdi4Request()
    {
    }

    public Cfdi4Request(Emisor emisor, Receptor receptor, List<Concepto> conceptos)
    {
        Emisor = emisor;
        Receptor = receptor;
        Conceptos = conceptos;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["emisor"] = Emisor.ToDictionary(),
            ["receptor"] = Receptor.ToDictionary(),
            ["conceptos"] = Conceptos.Select(c => c.ToDictionary()).ToList(),
            ["tipo_comprobante"] = TipoComprobante,
            ["forma_pago"] = FormaPago,
            ["metodo_pago"] = MetodoPago,
            ["moneda"] = Moneda,
            ["tipo_cambio"] = TipoCambio,
            ["condiciones_pago"] = CondicionesPago,
            ["subtotal"] = Subtotal,
            ["descuento"] = Descuento,
            ["total"] = Total,
            ["impuestos"] = Impuestos?.ToDictionary(),
            ["cfdi_relacionados"] = CfdiRelacionados?.Select(c => c.ToDictionary()).ToList(),
            ["informacion_global"] = InformacionGlobal?.ToDictionary(),
            ["exportacion"] = Exportacion,
            ["fecha"] = Fecha,
            ["lugar_expedicion"] = LugarExpedicion
        };
    }
}
