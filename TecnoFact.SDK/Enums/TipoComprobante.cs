namespace TecnoFact.SDK.Enums;

/// <summary>
/// Tipos de comprobante CFDI
/// </summary>
public enum TipoComprobante
{
    /// <summary>
    /// Ingreso (I)
    /// </summary>
    Ingreso,
    
    /// <summary>
    /// Egreso (E)
    /// </summary>
    Egreso,
    
    /// <summary>
    /// Traslado (T)
    /// </summary>
    Traslado,
    
    /// <summary>
    /// Nómina (N)
    /// </summary>
    Nomina,
    
    /// <summary>
    /// Pago (P)
    /// </summary>
    Pago
}

/// <summary>
/// Métodos de extensión para el enum TipoComprobante
/// </summary>
public static class TipoComprobanteExtensions
{
    /// <summary>
    /// Obtiene el código del tipo de comprobante
    /// </summary>
    public static string GetCode(this TipoComprobante tipo)
        => tipo switch
        {
            TipoComprobante.Ingreso => "I",
            TipoComprobante.Egreso => "E",
            TipoComprobante.Traslado => "T",
            TipoComprobante.Nomina => "N",
            TipoComprobante.Pago => "P",
            _ => throw new ArgumentOutOfRangeException(nameof(tipo))
        };

    /// <summary>
    /// Obtiene la descripción del tipo de comprobante
    /// </summary>
    public static string GetDescription(this TipoComprobante tipo)
        => tipo switch
        {
            TipoComprobante.Ingreso => "Ingreso",
            TipoComprobante.Egreso => "Egreso",
            TipoComprobante.Traslado => "Traslado",
            TipoComprobante.Nomina => "Nómina",
            TipoComprobante.Pago => "Pago",
            _ => throw new ArgumentOutOfRangeException(nameof(tipo))
        };
}
