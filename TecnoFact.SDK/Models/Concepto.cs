using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Concepto o partida de un CFDI
/// </summary>
public class Concepto
{
    /// <summary>
    /// Clave del producto o servicio
    /// </summary>
    [JsonPropertyName("clave_prod_serv")]
    public string ClaveProdServ { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad
    /// </summary>
    [JsonPropertyName("cantidad")]
    public decimal Cantidad { get; set; }

    /// <summary>
    /// Clave de unidad
    /// </summary>
    [JsonPropertyName("clave_unidad")]
    public string ClaveUnidad { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del concepto
    /// </summary>
    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Valor unitario
    /// </summary>
    [JsonPropertyName("valor_unitario")]
    public decimal ValorUnitario { get; set; }

    /// <summary>
    /// Importe total del concepto
    /// </summary>
    [JsonPropertyName("importe")]
    public decimal Importe { get; set; }

    /// <summary>
    /// Objeto de impuesto (01: No objeto, 02: Sí objeto, 03: Sí objeto - No obligado, 04: Sí objeto - Actividad gravada)
    /// </summary>
    [JsonPropertyName("objeto_imp")]
    public string? ObjetoImp { get; set; }

    /// <summary>
    /// Descuento aplicado al concepto
    /// </summary>
    [JsonPropertyName("descuento")]
    public decimal? Descuento { get; set; }

    /// <summary>
    /// Unidad de medida
    /// </summary>
    [JsonPropertyName("unidad")]
    public string? Unidad { get; set; }

    /// <summary>
    /// Número de identificación
    /// </summary>
    [JsonPropertyName("no_identificacion")]
    public string? NoIdentificacion { get; set; }

    /// <summary>
    /// Impuestos del concepto
    /// </summary>
    [JsonPropertyName("impuestos")]
    public ImpuestosConcepto? Impuestos { get; set; }

    public Concepto()
    {
    }

    public Concepto(string claveProdServ, decimal cantidad, string claveUnidad, string descripcion, 
                   decimal valorUnitario, decimal importe, string? objetoImp = null, 
                   decimal? descuento = null, string? unidad = null, string? noIdentificacion = null,
                   ImpuestosConcepto? impuestos = null)
    {
        ClaveProdServ = claveProdServ;
        Cantidad = cantidad;
        ClaveUnidad = claveUnidad;
        Descripcion = descripcion;
        ValorUnitario = valorUnitario;
        Importe = importe;
        ObjetoImp = objetoImp;
        Descuento = descuento;
        Unidad = unidad;
        NoIdentificacion = noIdentificacion;
        Impuestos = impuestos;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["clave_prod_serv"] = ClaveProdServ,
            ["cantidad"] = Cantidad,
            ["clave_unidad"] = ClaveUnidad,
            ["descripcion"] = Descripcion,
            ["valor_unitario"] = ValorUnitario,
            ["importe"] = Importe,
            ["objeto_imp"] = ObjetoImp,
            ["descuento"] = Descuento,
            ["unidad"] = Unidad,
            ["no_identificacion"] = NoIdentificacion,
            ["impuestos"] = Impuestos?.ToDictionary()
        };
    }
}
