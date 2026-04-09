using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Datos del receptor del CFDI
/// </summary>
public class Receptor
{
    /// <summary>
    /// RFC del receptor
    /// </summary>
    [JsonPropertyName("rfc")]
    public string Rfc { get; set; } = string.Empty;

    /// <summary>
    /// Nombre o razón social del receptor
    /// </summary>
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Uso del CFDI
    /// </summary>
    [JsonPropertyName("uso_cfdi")]
    public string UsoCfdi { get; set; } = string.Empty;

    /// <summary>
    /// Domicilio fiscal del receptor
    /// </summary>
    [JsonPropertyName("domicilio_fiscal_receptor")]
    public string? DomicilioFiscalReceptor { get; set; }

    /// <summary>
    /// Régimen fiscal del receptor
    /// </summary>
    [JsonPropertyName("regimen_fiscal_receptor")]
    public string? RegimenFiscalReceptor { get; set; }

    public Receptor()
    {
    }

    public Receptor(string rfc, string nombre, string usoCfdi, string? domicilioFiscalReceptor = null, string? regimenFiscalReceptor = null)
    {
        Rfc = rfc;
        Nombre = nombre;
        UsoCfdi = usoCfdi;
        DomicilioFiscalReceptor = domicilioFiscalReceptor;
        RegimenFiscalReceptor = regimenFiscalReceptor;
    }

    public string GetRfc() => Rfc;
    public string GetNombre() => Nombre;
    public string GetUsoCfdi() => UsoCfdi;
    public string? GetDomicilioFiscalReceptor() => DomicilioFiscalReceptor;
    public string? GetRegimenFiscalReceptor() => RegimenFiscalReceptor;

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["rfc"] = Rfc,
            ["nombre"] = Nombre,
            ["uso_cfdi"] = UsoCfdi,
            ["domicilio_fiscal_receptor"] = DomicilioFiscalReceptor,
            ["regimen_fiscal_receptor"] = RegimenFiscalReceptor
        };
    }
}
