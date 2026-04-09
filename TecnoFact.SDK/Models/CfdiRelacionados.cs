using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// CFDIs relacionados
/// </summary>
public class CfdiRelacionados
{
    /// <summary>
    /// Tipo de relación
    /// </summary>
    [JsonPropertyName("tipo_relacion")]
    public string TipoRelacion { get; set; } = string.Empty;

    /// <summary>
    /// Lista de UUIDs relacionados
    /// </summary>
    [JsonPropertyName("uuid")]
    public List<string> Uuid { get; set; } = new();

    public CfdiRelacionados()
    {
    }

    public CfdiRelacionados(string tipoRelacion, List<string> uuid)
    {
        TipoRelacion = tipoRelacion;
        Uuid = uuid;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            ["tipo_relacion"] = TipoRelacion,
            ["uuid"] = Uuid
        };
    }
}
