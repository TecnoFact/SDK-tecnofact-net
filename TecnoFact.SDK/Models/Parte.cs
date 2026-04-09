using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Parte o componente de un concepto
/// </summary>
public class Parte
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
    /// Descripción
    /// </summary>
    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Valor unitario
    /// </summary>
    [JsonPropertyName("valor_unitario")]
    public decimal? ValorUnitario { get; set; }

    /// <summary>
    /// Importe
    /// </summary>
    [JsonPropertyName("importe")]
    public decimal? Importe { get; set; }

    /// <summary>
    /// Número de identificación
    /// </summary>
    [JsonPropertyName("no_identificacion")]
    public string? NoIdentificacion { get; set; }

    /// <summary>
    /// Unidad
    /// </summary>
    [JsonPropertyName("unidad")]
    public string? Unidad { get; set; }

    public Parte()
    {
    }

    public Parte(string claveProdServ, decimal cantidad, string descripcion, 
                decimal? valorUnitario = null, decimal? importe = null, 
                string? noIdentificacion = null, string? unidad = null)
    {
        ClaveProdServ = claveProdServ;
        Cantidad = cantidad;
        Descripcion = descripcion;
        ValorUnitario = valorUnitario;
        Importe = importe;
        NoIdentificacion = noIdentificacion;
        Unidad = unidad;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["clave_prod_serv"] = ClaveProdServ,
            ["cantidad"] = Cantidad,
            ["descripcion"] = Descripcion,
            ["valor_unitario"] = ValorUnitario,
            ["importe"] = Importe,
            ["no_identificacion"] = NoIdentificacion,
            ["unidad"] = Unidad
        };
    }
}
