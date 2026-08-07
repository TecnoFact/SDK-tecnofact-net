using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Typed result returned after CFDI stamping.
/// </summary>
public sealed class ResultadoTimbrado
{
    private string? _message;
    private string? _error;
    private string? _xmlTimbrado;
    private string? _uuid;

    /// <summary>
    /// Gets or sets whether the stamping operation succeeded.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the response code.
    /// </summary>
    [JsonPropertyName("code")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Code { get; set; }

    /// <summary>
    /// Gets or sets the response message, falling back to the error field.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message
    {
        get => _message ?? _error;
        set => _message = value;
    }

    /// <summary>
    /// Gets or sets the panel error message.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error
    {
        get => _error;
        set => _error = value;
    }

    /// <summary>
    /// Gets or sets the stamped XML, including supported nested response fields.
    /// </summary>
    [JsonPropertyName("xml_timbrado")]
    public string? XmlTimbrado
    {
        get => _xmlTimbrado ?? Data?.XmlTimbrado ?? Data?.Xml;
        set => _xmlTimbrado = value;
    }

    /// <summary>
    /// Gets or sets the fiscal UUID, including the supported nested response field.
    /// </summary>
    [JsonPropertyName("uuid")]
    public string? Uuid
    {
        get => _uuid ?? Data?.Uuid;
        set => _uuid = value;
    }

    /// <summary>
    /// Gets or sets nested response data from the panel.
    /// </summary>
    [JsonPropertyName("data")]
    public ResultadoTimbradoData? Data { get; set; }
}

/// <summary>
/// Nested stamping result fields returned by the panel.
/// </summary>
public sealed class ResultadoTimbradoData
{
    /// <summary>
    /// Gets or sets the stamped XML.
    /// </summary>
    [JsonPropertyName("xml_timbrado")]
    public string? XmlTimbrado { get; set; }

    /// <summary>
    /// Gets or sets the alternate XML field.
    /// </summary>
    [JsonPropertyName("xml")]
    public string? Xml { get; set; }

    /// <summary>
    /// Gets or sets the fiscal UUID.
    /// </summary>
    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}
