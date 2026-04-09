using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Datos del emisor del CFDI
/// </summary>
public class Emisor
{
    /// <summary>
    /// RFC del emisor
    /// </summary>
    [JsonPropertyName("rfc")]
    public string Rfc { get; set; } = string.Empty;

    /// <summary>
    /// Nombre o razón social del emisor
    /// </summary>
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Régimen fiscal del emisor
    /// </summary>
    [JsonPropertyName("regimen_fiscal")]
    public string RegimenFiscal { get; set; } = string.Empty;

    /// <summary>
    /// Código postal del emisor
    /// </summary>
    [JsonPropertyName("cp")]
    public string Cp { get; set; } = string.Empty;

    public Emisor()
    {
    }

    public Emisor(string rfc, string nombre, string regimenFiscal, string cp)
    {
        Rfc = rfc;
        Nombre = nombre;
        RegimenFiscal = regimenFiscal;
        Cp = cp;
    }

    public string GetRfc() => Rfc;
    public string GetNombre() => Nombre;
    public string GetRegimenFiscal() => RegimenFiscal;
    public string GetCp() => Cp;

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["rfc"] = Rfc,
            ["nombre"] = Nombre,
            ["regimen_fiscal"] = RegimenFiscal,
            ["cp"] = Cp
        };
    }
}
