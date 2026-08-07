using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using TecnoFact.SDK.Models;

namespace TecnoFact.SDK.Xml;

/// <summary>
/// Builds the unsigned structural XML required for CFDI 4.0 I/E panel stamping.
/// </summary>
public sealed class CfdiXmlBuilder
{
    private static readonly XNamespace Cfdi = "http://www.sat.gob.mx/cfd/4";
    private static readonly XNamespace Xsi = "http://www.w3.org/2001/XMLSchema-instance";
    private const string SchemaLocation = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";

    /// <summary>
    /// Builds an unsigned CFDI 4.0 XML document from the supplied request.
    /// </summary>
    public string Build(Cfdi4Request request)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validate(request);

        var comprobante = new XElement(
            Cfdi + "Comprobante",
            new XAttribute(XNamespace.Xmlns + "cfdi", Cfdi.NamespaceName),
            new XAttribute(XNamespace.Xmlns + "xsi", Xsi.NamespaceName),
            new XAttribute(Xsi + "schemaLocation", SchemaLocation),
            new XAttribute("Version", "4.0"),
            new XAttribute("Fecha", request.Fecha!.Value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)),
            new XAttribute("FormaPago", request.FormaPago!),
            request.CondicionesPago is not null ? new XAttribute("CondicionesDePago", request.CondicionesPago) : null,
            new XAttribute("SubTotal", Amount(request.Subtotal!.Value)),
            request.Descuento is decimal descuento ? new XAttribute("Descuento", Amount(descuento)) : null,
            new XAttribute("Moneda", request.Moneda!),
            request.TipoCambio is decimal tipoCambio ? new XAttribute("TipoCambio", Quantity(tipoCambio)) : null,
            new XAttribute("Total", Amount(request.Total!.Value)),
            new XAttribute("TipoDeComprobante", request.TipoComprobante!),
            new XAttribute("Exportacion", request.Exportacion!),
            new XAttribute("MetodoPago", request.MetodoPago!),
            new XAttribute("LugarExpedicion", request.LugarExpedicion!),
            request.InformacionGlobal is not null ? InformacionGlobal(request.InformacionGlobal) : null,
            request.CfdiRelacionados?.Select(CfdiRelacionados),
            Emisor(request.Emisor),
            Receptor(request.Receptor),
            Conceptos(request.Conceptos));

        if (request.Impuestos is not null)
        {
            comprobante.Add(Impuestos(request.Impuestos));
        }

        var document = new XDocument(new XDeclaration("1.0", "UTF-8", null), comprobante);
        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            Indent = true,
            OmitXmlDeclaration = false
        };

        using var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, settings))
        {
            document.Save(writer);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static XElement Emisor(Emisor emisor) => new(
        Cfdi + "Emisor",
        new XAttribute("Rfc", emisor.Rfc),
        new XAttribute("Nombre", emisor.Nombre),
        new XAttribute("RegimenFiscal", emisor.RegimenFiscal));

    private static XElement Receptor(Receptor receptor) => new(
        Cfdi + "Receptor",
        new XAttribute("Rfc", receptor.Rfc),
        new XAttribute("Nombre", receptor.Nombre),
        new XAttribute("DomicilioFiscalReceptor", receptor.DomicilioFiscalReceptor!),
        new XAttribute("RegimenFiscalReceptor", receptor.RegimenFiscalReceptor!),
        new XAttribute("UsoCFDI", receptor.UsoCfdi));

    private static XElement CfdiRelacionados(CfdiRelacionados relacionados) => new(
        Cfdi + "CfdiRelacionados",
        new XAttribute("TipoRelacion", relacionados.TipoRelacion),
        relacionados.Uuid.Select(uuid => new XElement(
            Cfdi + "CfdiRelacionado",
            new XAttribute("UUID", uuid))));

    private static XElement InformacionGlobal(InformacionGlobal informacionGlobal) => new(
        Cfdi + "InformacionGlobal",
        new XAttribute("Periodicidad", informacionGlobal.Periodicidad),
        new XAttribute("Meses", informacionGlobal.Meses),
        new XAttribute("Año", informacionGlobal.Anio));

    private static XElement Conceptos(IEnumerable<Concepto> conceptos) => new(
        Cfdi + "Conceptos",
        conceptos.Select(Concepto));

    private static XElement Concepto(Concepto concepto)
    {
        var node = new XElement(
            Cfdi + "Concepto",
            new XAttribute("ClaveProdServ", concepto.ClaveProdServ),
            new XAttribute("Cantidad", Quantity(concepto.Cantidad)),
            new XAttribute("ClaveUnidad", concepto.ClaveUnidad),
            new XAttribute("Descripcion", concepto.Descripcion),
            new XAttribute("ValorUnitario", Amount(concepto.ValorUnitario)),
            new XAttribute("Importe", Amount(concepto.Importe)),
            new XAttribute("ObjetoImp", concepto.ObjetoImp!));

        if (!string.IsNullOrWhiteSpace(concepto.NoIdentificacion))
        {
            node.Add(new XAttribute("NoIdentificacion", concepto.NoIdentificacion));
        }

        if (!string.IsNullOrWhiteSpace(concepto.Unidad))
        {
            node.Add(new XAttribute("Unidad", concepto.Unidad));
        }

        if (concepto.Descuento is not null)
        {
            node.Add(new XAttribute("Descuento", Amount(concepto.Descuento.Value)));
        }

        if (concepto.ObjetoImp == "02" && concepto.Impuestos is not null)
        {
            var impuestos = ImpuestosConcepto(concepto.Impuestos);
            if (impuestos.HasElements)
            {
                node.Add(impuestos);
            }
        }

        if (concepto.ACuentaTerceros is not null)
        {
            node.Add(ACuentaTerceros(concepto.ACuentaTerceros));
        }

        if (concepto.InformacionAduanera is not null)
        {
            node.Add(concepto.InformacionAduanera.Select(InformacionAduanera));
        }

        if (concepto.CuentaPredial is not null)
        {
            node.Add(concepto.CuentaPredial.Select(CuentaPredial));
        }

        return node;
    }

    private static XElement InformacionAduanera(InformacionAduanera informacionAduanera) => new(
        Cfdi + "InformacionAduanera",
        new XAttribute("NumeroPedimento", informacionAduanera.NumeroPedimento));

    private static XElement CuentaPredial(CuentaPredial cuentaPredial) => new(
        Cfdi + "CuentaPredial",
        new XAttribute("Numero", cuentaPredial.Numero));

    private static XElement ACuentaTerceros(ACuentaTerceros aCuentaTerceros) => new(
        Cfdi + "ACuentaTerceros",
        new XAttribute("RfcACuentaTerceros", aCuentaTerceros.RfcACuentaTerceros),
        new XAttribute("NombreACuentaTerceros", aCuentaTerceros.NombreACuentaTerceros),
        new XAttribute("RegimenFiscalACuentaTerceros", aCuentaTerceros.RegimenFiscalACuentaTerceros),
        new XAttribute("DomicilioFiscalACuentaTerceros", aCuentaTerceros.DomicilioFiscalACuentaTerceros));

    private static XElement ImpuestosConcepto(ImpuestosConcepto impuestos)
    {
        var node = new XElement(Cfdi + "Impuestos");

        if (impuestos.Traslados is { Count: > 0 })
        {
            node.Add(new XElement(Cfdi + "Traslados", impuestos.Traslados.Select(Traslado)));
        }

        if (impuestos.Retenciones is { Count: > 0 })
        {
            node.Add(new XElement(Cfdi + "Retenciones", impuestos.Retenciones.Select(Retencion)));
        }

        return node;
    }

    private static XElement Traslado(Traslado traslado)
    {
        var node = new XElement(
            Cfdi + "Traslado",
            new XAttribute("Base", Amount(traslado.Base)),
            new XAttribute("Impuesto", traslado.Impuesto),
            new XAttribute("TipoFactor", traslado.TipoFactor));

        if (traslado.TipoFactor != "Exento")
        {
            if (traslado.TasaOCuota is not null)
            {
                node.Add(new XAttribute("TasaOCuota", Quantity(traslado.TasaOCuota.Value)));
            }

            if (traslado.Importe is not null)
            {
                node.Add(new XAttribute("Importe", Amount(traslado.Importe.Value)));
            }
        }

        return node;
    }

    private static XElement Retencion(Retencion retencion) => new(
        Cfdi + "Retencion",
        new XAttribute("Base", Amount(retencion.Base)),
        new XAttribute("Impuesto", retencion.Impuesto),
        new XAttribute("TipoFactor", retencion.TipoFactor),
        new XAttribute("TasaOCuota", Quantity(retencion.TasaOCuota)),
        new XAttribute("Importe", Amount(retencion.Importe)));

    private static XElement Impuestos(Impuestos impuestos)
    {
        var node = new XElement(Cfdi + "Impuestos");

        if (impuestos.TotalImpuestosRetenidos is not null)
        {
            node.Add(new XAttribute("TotalImpuestosRetenidos", Amount(impuestos.TotalImpuestosRetenidos.Value)));
        }

        if (impuestos.TotalImpuestosTrasladados is not null)
        {
            node.Add(new XAttribute("TotalImpuestosTrasladados", Amount(impuestos.TotalImpuestosTrasladados.Value)));
        }

        if (impuestos.Retenciones is { Count: > 0 })
        {
            node.Add(new XElement(Cfdi + "Retenciones", impuestos.Retenciones.Select(retencion => new XElement(
                Cfdi + "Retencion",
                new XAttribute("Impuesto", retencion.Impuesto),
                new XAttribute("Importe", Amount(retencion.Importe))))));
        }

        if (impuestos.Traslados is { Count: > 0 })
        {
            node.Add(new XElement(Cfdi + "Traslados", impuestos.Traslados.Select(TrasladoGlobal)));
        }

        return node;
    }

    private static XElement TrasladoGlobal(TrasladoGlobal traslado)
    {
        var node = new XElement(
            Cfdi + "Traslado",
            new XAttribute("Base", Amount(traslado.Base)),
            new XAttribute("Impuesto", traslado.Impuesto),
            new XAttribute("TipoFactor", traslado.TipoFactor));

        if (traslado.TipoFactor != "Exento")
        {
            if (traslado.TasaOCuota is not null)
            {
                node.Add(new XAttribute("TasaOCuota", Quantity(traslado.TasaOCuota.Value)));
            }

            if (traslado.Importe is not null)
            {
                node.Add(new XAttribute("Importe", Amount(traslado.Importe.Value)));
            }
        }

        return node;
    }

    private static void Validate(Cfdi4Request request)
    {
        RequireValue(request.Fecha, nameof(request.Fecha));
        RequireValue(request.LugarExpedicion, nameof(request.LugarExpedicion));
        RequireValue(request.TipoComprobante, nameof(request.TipoComprobante));
        RequireValue(request.FormaPago, nameof(request.FormaPago));
        RequireValue(request.MetodoPago, nameof(request.MetodoPago));
        RequireValue(request.Moneda, nameof(request.Moneda));
        RequireValue(request.Subtotal, nameof(request.Subtotal));
        RequireValue(request.Total, nameof(request.Total));
        RequireValue(request.Exportacion, nameof(request.Exportacion));
        RequireValue(request.Emisor, nameof(request.Emisor));
        RequireValue(request.Receptor, nameof(request.Receptor));
        RequireValue(request.Conceptos, nameof(request.Conceptos));

        if (request.TipoComprobante is not ("I" or "E"))
        {
            throw new ArgumentException("TipoComprobante must be I or E.", nameof(request.TipoComprobante));
        }

        if (request.Conceptos.Count == 0)
        {
            throw new ArgumentException("Conceptos must contain at least one item.", nameof(request.Conceptos));
        }

        RequireValue(request.Emisor.Rfc, nameof(request.Emisor.Rfc));
        RequireValue(request.Emisor.Nombre, nameof(request.Emisor.Nombre));
        RequireValue(request.Emisor.RegimenFiscal, nameof(request.Emisor.RegimenFiscal));
        RequireValue(request.Receptor.Rfc, nameof(request.Receptor.Rfc));
        RequireValue(request.Receptor.Nombre, nameof(request.Receptor.Nombre));
        RequireValue(request.Receptor.DomicilioFiscalReceptor, nameof(request.Receptor.DomicilioFiscalReceptor));
        RequireValue(request.Receptor.RegimenFiscalReceptor, nameof(request.Receptor.RegimenFiscalReceptor));
        RequireValue(request.Receptor.UsoCfdi, nameof(request.Receptor.UsoCfdi));

        foreach (var concepto in request.Conceptos)
        {
            RequireValue(concepto, nameof(request.Conceptos));
            RequireValue(concepto.ClaveProdServ, nameof(concepto.ClaveProdServ));
            RequireValue(concepto.ClaveUnidad, nameof(concepto.ClaveUnidad));
            RequireValue(concepto.Descripcion, nameof(concepto.Descripcion));
            RequireValue(concepto.ObjetoImp, nameof(concepto.ObjetoImp));

            if (concepto.ACuentaTerceros is not null)
            {
                RequireValue(concepto.ACuentaTerceros.RfcACuentaTerceros, nameof(concepto.ACuentaTerceros.RfcACuentaTerceros));
                RequireValue(concepto.ACuentaTerceros.NombreACuentaTerceros, nameof(concepto.ACuentaTerceros.NombreACuentaTerceros));
                RequireValue(concepto.ACuentaTerceros.RegimenFiscalACuentaTerceros, nameof(concepto.ACuentaTerceros.RegimenFiscalACuentaTerceros));
                RequireValue(concepto.ACuentaTerceros.DomicilioFiscalACuentaTerceros, nameof(concepto.ACuentaTerceros.DomicilioFiscalACuentaTerceros));
            }

            if (concepto.InformacionAduanera is not null)
            {
                foreach (var informacionAduanera in concepto.InformacionAduanera)
                {
                    RequireValue(informacionAduanera, nameof(concepto.InformacionAduanera));
                    RequireValue(informacionAduanera.NumeroPedimento, nameof(informacionAduanera.NumeroPedimento));

                    if (!Regex.IsMatch(informacionAduanera.NumeroPedimento, @"^\d{2}  \d{2}  \d{4}  \d{7}$"))
                    {
                        throw new ArgumentException(
                            "InformacionAduanera NumeroPedimento must match the CFDI 4.0 pedimento format.",
                            nameof(informacionAduanera.NumeroPedimento));
                    }
                }
            }

            if (concepto.CuentaPredial is not null)
            {
                foreach (var cuentaPredial in concepto.CuentaPredial)
                {
                    RequireValue(cuentaPredial, nameof(concepto.CuentaPredial));
                    RequireValue(cuentaPredial.Numero, nameof(cuentaPredial.Numero));

                    if (!Regex.IsMatch(cuentaPredial.Numero, "^[0-9a-zA-Z]{1,150}$"))
                    {
                        throw new ArgumentException(
                            "CuentaPredial Numero must contain between 1 and 150 alphanumeric characters.",
                            nameof(cuentaPredial.Numero));
                    }
                }
            }
        }

        if (request.CfdiRelacionados is not null)
        {
            foreach (var relacionados in request.CfdiRelacionados)
            {
                RequireValue(relacionados, nameof(request.CfdiRelacionados));
                RequireValue(relacionados.TipoRelacion, nameof(relacionados.TipoRelacion));

                if (relacionados.Uuid is not { Count: > 0 })
                {
                    throw new ArgumentException("CfdiRelacionados must contain at least one UUID.", nameof(request.CfdiRelacionados));
                }

                foreach (var uuid in relacionados.Uuid)
                {
                    if (string.IsNullOrWhiteSpace(uuid) || !Guid.TryParseExact(uuid, "D", out _))
                    {
                        throw new ArgumentException("CfdiRelacionado UUID must be a valid UUID.", nameof(relacionados.Uuid));
                    }
                }
            }
        }

        if (request.InformacionGlobal is not null)
        {
            RequireValue(request.InformacionGlobal.Periodicidad, nameof(request.InformacionGlobal.Periodicidad));
            RequireValue(request.InformacionGlobal.Meses, nameof(request.InformacionGlobal.Meses));
            RequireValue(request.InformacionGlobal.Anio, nameof(request.InformacionGlobal.Anio));
        }
    }

    private static void RequireValue<T>(T? value, string parameterName)
    {
        if (value is null || value is string text && string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException($"{parameterName} is required.", parameterName);
        }
    }

    private static string Amount(decimal value) => value.ToString("0.00", CultureInfo.InvariantCulture);

    private static string Quantity(decimal value) => value.ToString("0.######", CultureInfo.InvariantCulture);
}
