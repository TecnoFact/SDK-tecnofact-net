using System.Text.Json.Serialization;

namespace TecnoFact.SDK.Models;

/// <summary>
/// Request payload for XML CFDI stamping.
/// </summary>
public sealed class TimbrarXmlRequest
{
    /// <summary>
    /// Gets the CFDI XML to stamp.
    /// </summary>
    [JsonPropertyName("xml")]
    public string Xml { get; }

    /// <summary>
    /// Creates an XML CFDI stamping request.
    /// </summary>
    public TimbrarXmlRequest(string xml)
    {
        Xml = xml ?? throw new ArgumentNullException(nameof(xml));
    }
}
