using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Third-party taxpayer on whose behalf the operation is performed.
/// </summary>
public class ACuentaTerceros
{
    /// <summary>
    /// RFC of the third-party taxpayer.
    /// </summary>
    [JsonPropertyName("rfc_a_cuenta_terceros")]
    public string RfcACuentaTerceros { get; set; } = string.Empty;

    /// <summary>
    /// Name of the third-party taxpayer.
    /// </summary>
    [JsonPropertyName("nombre_a_cuenta_terceros")]
    public string NombreACuentaTerceros { get; set; } = string.Empty;

    /// <summary>
    /// Tax regime of the third-party taxpayer.
    /// </summary>
    [JsonPropertyName("regimen_fiscal_a_cuenta_terceros")]
    public string RegimenFiscalACuentaTerceros { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal domicile postal code of the third-party taxpayer.
    /// </summary>
    [JsonPropertyName("domicilio_fiscal_a_cuenta_terceros")]
    public string DomicilioFiscalACuentaTerceros { get; set; } = string.Empty;

    /// <summary>
    /// Converts the model to the SDK request dictionary format.
    /// </summary>
    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["rfc_a_cuenta_terceros"] = RfcACuentaTerceros,
            ["nombre_a_cuenta_terceros"] = NombreACuentaTerceros,
            ["regimen_fiscal_a_cuenta_terceros"] = RegimenFiscalACuentaTerceros,
            ["domicilio_fiscal_a_cuenta_terceros"] = DomicilioFiscalACuentaTerceros
        };
    }
}
