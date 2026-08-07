using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Global invoice information for public-general operations.
/// </summary>
public class InformacionGlobal
{
    [JsonPropertyName("periodicidad")]
    public string Periodicidad { get; set; } = string.Empty;

    [JsonPropertyName("meses")]
    public string Meses { get; set; } = string.Empty;

    [JsonPropertyName("anio")]
    public string Anio { get; set; } = string.Empty;

    public InformacionGlobal()
    {
    }

    public InformacionGlobal(string periodicidad, string meses, string anio)
    {
        Periodicidad = periodicidad;
        Meses = meses;
        Anio = anio;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["periodicidad"] = Periodicidad,
            ["meses"] = Meses,
            ["anio"] = Anio
        };
    }
}
