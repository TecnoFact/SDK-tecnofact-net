using System.Xml.Linq;
using TecnoFact.SDK.Models;
using TecnoFact.SDK.Xml;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class CfdiXmlBuilderTests
{
    private static readonly XNamespace Cfdi = "http://www.sat.gob.mx/cfd/4";

    [Theory]
    [InlineData("I")]
    [InlineData("E")]
    public void Build_MinimalIngresoOrEgreso_ProducesUnsignedCfdiInStableOrderAndFormat(string tipoComprobante)
    {
        var request = CreateRequest();
        request.TipoComprobante = tipoComprobante;

        var xml = new CfdiXmlBuilder().Build(request);
        var document = XDocument.Parse(xml);
        var comprobante = Assert.IsType<XElement>(document.Root);

        Assert.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xml, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(Cfdi + "Comprobante", comprobante.Name);
        Assert.Equal("http://www.sat.gob.mx/cfd/4", comprobante.GetNamespaceOfPrefix("cfdi")?.NamespaceName);
        Assert.Equal("http://www.w3.org/2001/XMLSchema-instance", comprobante.GetNamespaceOfPrefix("xsi")?.NamespaceName);
        Assert.Equal("http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd", comprobante.Attribute(XName.Get("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance"))?.Value);
        Assert.Equal("4.0", comprobante.Attribute("Version")?.Value);
        Assert.Equal(tipoComprobante, comprobante.Attribute("TipoDeComprobante")?.Value);
        Assert.Equal("2025-02-04T13:38:31", comprobante.Attribute("Fecha")?.Value);
        Assert.Equal("100.00", comprobante.Attribute("SubTotal")?.Value);
        Assert.Equal("100.00", comprobante.Attribute("Total")?.Value);
        Assert.Equal("1.25", comprobante.Attribute("TipoCambio")?.Value);
        Assert.Equal(new[] { Cfdi + "Emisor", Cfdi + "Receptor", Cfdi + "Conceptos" }, comprobante.Elements().Select(element => element.Name));

        var concepto = comprobante.Element(Cfdi + "Conceptos")!.Element(Cfdi + "Concepto")!;
        Assert.Equal("1.5", concepto.Attribute("Cantidad")?.Value);
        Assert.DoesNotContain("Sello", comprobante.Attributes().Select(attribute => attribute.Name.LocalName));
        Assert.DoesNotContain("NoCertificado", comprobante.Attributes().Select(attribute => attribute.Name.LocalName));
        Assert.DoesNotContain("Certificado", comprobante.Attributes().Select(attribute => attribute.Name.LocalName));
        Assert.Null(comprobante.Element(Cfdi + "Complemento"));
    }

    [Fact]
    public void Build_ConceptTaxesAndGlobalTaxes_UsesSchemaOrderAndOmitsExentoRateAndAmount()
    {
        var request = CreateRequest();
        request.Conceptos[0].Impuestos = new ImpuestosConcepto(
            traslados: new List<Traslado>
            {
                new(100m, "002", "Tasa", 0.16m, 16m),
                new(50m, "002", "Exento")
            });
        request.Impuestos = new Impuestos(
            totalImpuestosTrasladados: 16m,
            traslados: new List<TrasladoGlobal>
            {
                new(150m, "002", "Tasa", 0.16m, 16m)
            });

        var comprobante = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!;
        var conceptTraslados = comprobante
            .Element(Cfdi + "Conceptos")!
            .Element(Cfdi + "Concepto")!
            .Element(Cfdi + "Impuestos")!
            .Element(Cfdi + "Traslados")!
            .Elements(Cfdi + "Traslado")
            .ToList();
        var exento = conceptTraslados[1];
        var globalTraslado = comprobante
            .Element(Cfdi + "Impuestos")!
            .Element(Cfdi + "Traslados")!
            .Element(Cfdi + "Traslado")!;

        Assert.Equal("100.00", conceptTraslados[0].Attribute("Base")?.Value);
        Assert.Equal("0.16", conceptTraslados[0].Attribute("TasaOCuota")?.Value);
        Assert.Equal("16.00", conceptTraslados[0].Attribute("Importe")?.Value);
        Assert.Equal("Exento", exento.Attribute("TipoFactor")?.Value);
        Assert.Null(exento.Attribute("TasaOCuota"));
        Assert.Null(exento.Attribute("Importe"));
        Assert.Equal("150.00", globalTraslado.Attribute("Base")?.Value);
        Assert.Equal("16.00", comprobante.Element(Cfdi + "Impuestos")!.Attribute("TotalImpuestosTrasladados")?.Value);
    }

    [Fact]
    public void Build_RootPaymentConditionsAndDiscount_UsesPhpAttributeOrderAndAmountFormat()
    {
        var request = CreateRequest();
        request.CondicionesPago = "Pago en una sola exhibicion";
        request.Descuento = 12.5m;

        var comprobante = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!;

        Assert.Equal("Pago en una sola exhibicion", comprobante.Attribute("CondicionesDePago")?.Value);
        Assert.Equal("12.50", comprobante.Attribute("Descuento")?.Value);
        Assert.Equal(
            new[]
            {
                "cfdi", "xsi", "schemaLocation", "Version", "Fecha", "FormaPago", "CondicionesDePago",
                "SubTotal", "Descuento", "Moneda", "TipoCambio", "Total", "TipoDeComprobante",
                "Exportacion", "MetodoPago", "LugarExpedicion"
            },
            comprobante.Attributes().Select(attribute => attribute.Name.LocalName));
    }

    [Fact]
    public void Build_CfdiRelacionados_EmitsRelatedUuidsBeforeEmisorInSchemaOrder()
    {
        var request = CreateRequest();
        request.CfdiRelacionados = new List<CfdiRelacionados>
        {
            new("01", new List<string>
            {
                "12345678-1234-1234-1234-123456789012",
                "87654321-4321-4321-4321-210987654321"
            })
        };

        var comprobante = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!;
        var relacionados = comprobante.Element(Cfdi + "CfdiRelacionados")!;

        Assert.Equal(
            new[] { Cfdi + "CfdiRelacionados", Cfdi + "Emisor", Cfdi + "Receptor", Cfdi + "Conceptos" },
            comprobante.Elements().Select(element => element.Name));
        Assert.Equal("01", relacionados.Attribute("TipoRelacion")?.Value);
        Assert.Equal(
            new[] { "12345678-1234-1234-1234-123456789012", "87654321-4321-4321-4321-210987654321" },
            relacionados.Elements(Cfdi + "CfdiRelacionado").Select(element => element.Attribute("UUID")?.Value));
    }

    [Fact]
    public void Build_InformacionGlobal_EmitsOnlyWhenProvidedBeforeRelatedCfdisAndEmisor()
    {
        var request = CreateRequest();
        request.InformacionGlobal = new InformacionGlobal("01", "08", "2025");
        request.CfdiRelacionados = new List<CfdiRelacionados>
        {
            new("01", new List<string> { "12345678-1234-1234-1234-123456789012" })
        };

        var comprobante = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!;
        var informacionGlobal = comprobante.Element(Cfdi + "InformacionGlobal")!;

        Assert.Equal(
            new[] { Cfdi + "InformacionGlobal", Cfdi + "CfdiRelacionados", Cfdi + "Emisor", Cfdi + "Receptor", Cfdi + "Conceptos" },
            comprobante.Elements().Select(element => element.Name));
        Assert.Equal("01", informacionGlobal.Attribute("Periodicidad")?.Value);
        Assert.Equal("08", informacionGlobal.Attribute("Meses")?.Value);
        Assert.Equal("2025", informacionGlobal.Attribute("Año")?.Value);
        Assert.Null(XDocument.Parse(new CfdiXmlBuilder().Build(CreateRequest())).Root!.Element(Cfdi + "InformacionGlobal"));
    }

    [Fact]
    public void Build_InformacionAduanera_EmitsZeroOrMoreNodesAfterTaxesForEachConcept()
    {
        var request = CreateRequest();
        request.Conceptos.Add(new Concepto("10101500", 1m, "H87", "Imported product", 200m, 200m, "02"));
        request.Conceptos[0].Impuestos = new ImpuestosConcepto(
            traslados: new List<Traslado> { new(100m, "002", "Tasa", 0.16m, 16m) });
        request.Conceptos[1].InformacionAduanera = new List<InformacionAduanera>
        {
            new("10  47  3807  8003832"),
            new("11  48  3808  9003833")
        };

        var conceptos = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!
            .Element(Cfdi + "Conceptos")!
            .Elements(Cfdi + "Concepto")
            .ToList();

        Assert.Empty(conceptos[0].Elements(Cfdi + "InformacionAduanera"));
        Assert.Equal(
            new[] { Cfdi + "Impuestos" },
            conceptos[0].Elements().Select(element => element.Name));
        Assert.Equal(
            new[] { Cfdi + "InformacionAduanera", Cfdi + "InformacionAduanera" },
            conceptos[1].Elements().Select(element => element.Name));
        Assert.Equal(
            new[] { "10  47  3807  8003832", "11  48  3808  9003833" },
            conceptos[1].Elements(Cfdi + "InformacionAduanera")
                .Select(element => element.Attribute("NumeroPedimento")?.Value));
    }

    [Fact]
    public void Build_ACuentaTerceros_EmitsOptionallyBetweenTaxesAndCustomsInformationWithAllAttributes()
    {
        var request = CreateRequest();
        request.Conceptos.Add(new Concepto("10101500", 1m, "H87", "Service", 200m, 200m, "02"));
        request.Conceptos[0].Impuestos = new ImpuestosConcepto(
            traslados: new List<Traslado> { new(100m, "002", "Tasa", 0.16m, 16m) });
        request.Conceptos[0].ACuentaTerceros = new ACuentaTerceros
        {
            RfcACuentaTerceros = "COSC8001137NA",
            NombreACuentaTerceros = "TERCERO",
            RegimenFiscalACuentaTerceros = "601",
            DomicilioFiscalACuentaTerceros = "20000"
        };
        request.Conceptos[0].InformacionAduanera = new List<InformacionAduanera>
        {
            new("10  47  3807  8003832")
        };

        var conceptos = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!
            .Element(Cfdi + "Conceptos")!
            .Elements(Cfdi + "Concepto")
            .ToList();
        var terceros = conceptos[0].Element(Cfdi + "ACuentaTerceros")!;

        Assert.Equal(
            new[] { Cfdi + "Impuestos", Cfdi + "ACuentaTerceros", Cfdi + "InformacionAduanera" },
            conceptos[0].Elements().Select(element => element.Name));
        Assert.Null(conceptos[1].Element(Cfdi + "ACuentaTerceros"));
        Assert.Equal("COSC8001137NA", terceros.Attribute("RfcACuentaTerceros")?.Value);
        Assert.Equal("TERCERO", terceros.Attribute("NombreACuentaTerceros")?.Value);
        Assert.Equal("601", terceros.Attribute("RegimenFiscalACuentaTerceros")?.Value);
        Assert.Equal("20000", terceros.Attribute("DomicilioFiscalACuentaTerceros")?.Value);
    }

    [Fact]
    public void Build_CuentaPredial_EmitsZeroOrMoreNodesAfterCustomsInformationWithNumeroAttribute()
    {
        var request = CreateRequest();
        request.Conceptos.Add(new Concepto("10101500", 1m, "H87", "Rental", 200m, 200m, "02"));
        request.Conceptos[1].Impuestos = new ImpuestosConcepto(
            traslados: new List<Traslado> { new(200m, "002", "Tasa", 0.16m, 32m) });
        request.Conceptos[1].ACuentaTerceros = new ACuentaTerceros
        {
            RfcACuentaTerceros = "COSC8001137NA",
            NombreACuentaTerceros = "TERCERO",
            RegimenFiscalACuentaTerceros = "601",
            DomicilioFiscalACuentaTerceros = "20000"
        };
        request.Conceptos[1].InformacionAduanera = new List<InformacionAduanera>
        {
            new("10  47  3807  8003832")
        };
        request.Conceptos[1].CuentaPredial = new List<CuentaPredial>
        {
            new("15956011002"),
            new("A123")
        };

        var conceptos = XDocument.Parse(new CfdiXmlBuilder().Build(request)).Root!
            .Element(Cfdi + "Conceptos")!
            .Elements(Cfdi + "Concepto")
            .ToList();

        Assert.Empty(conceptos[0].Elements(Cfdi + "CuentaPredial"));
        Assert.Equal(
            new[] { Cfdi + "Impuestos", Cfdi + "ACuentaTerceros", Cfdi + "InformacionAduanera", Cfdi + "CuentaPredial", Cfdi + "CuentaPredial" },
            conceptos[1].Elements().Select(element => element.Name));
        Assert.Equal(
            new[] { "15956011002", "A123" },
            conceptos[1].Elements(Cfdi + "CuentaPredial")
                .Select(element => element.Attribute("Numero")?.Value));
    }

    [Fact]
    public void Build_CuentaPredialWithoutNumero_ThrowsArgumentException()
    {
        var request = CreateRequest();
        request.Conceptos[0].CuentaPredial = new List<CuentaPredial> { new() };

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Fact]
    public void Build_CuentaPredialWithNonAlphanumericNumero_ThrowsArgumentException()
    {
        var request = CreateRequest();
        request.Conceptos[0].CuentaPredial = new List<CuentaPredial> { new("15956-011002") };

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Fact]
    public void Build_CuentaPredialWithNumeroLongerThan150Characters_ThrowsArgumentException()
    {
        var request = CreateRequest();
        request.Conceptos[0].CuentaPredial = new List<CuentaPredial> { new(new string('A', 151)) };

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Fact]
    public void Build_IncompleteACuentaTerceros_ThrowsArgumentException()
    {
        var request = CreateRequest();
        request.Conceptos[0].ACuentaTerceros = new ACuentaTerceros
        {
            RfcACuentaTerceros = "COSC8001137NA",
            NombreACuentaTerceros = "TERCERO",
            RegimenFiscalACuentaTerceros = "601"
        };

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Theory]
    [InlineData("")]
    [InlineData("10 47 3807 8003832")]
    [InlineData("10  47  3807  800383X")]
    public void Build_MissingOrMalformedInformacionAduanera_ThrowsArgumentException(string numeroPedimento)
    {
        var request = CreateRequest();
        request.Conceptos[0].InformacionAduanera = new List<InformacionAduanera> { new(numeroPedimento) };

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Theory]
    [InlineData("", "08", "2025")]
    [InlineData("01", "", "2025")]
    [InlineData("01", "08", "")]
    public void Build_PartialInformacionGlobalInput_ThrowsArgumentException(string periodicidad, string meses, string anio)
    {
        var request = CreateRequest();
        request.InformacionGlobal = new InformacionGlobal(periodicidad, meses, anio);

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Theory]
    [MemberData(nameof(InvalidCfdiRelacionados))]
    public void Build_InvalidOrMissingCfdiRelacionadosInput_ThrowsArgumentException(CfdiRelacionados relacionados)
    {
        var request = CreateRequest();
        request.CfdiRelacionados = new List<CfdiRelacionados> { relacionados };

        Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));
    }

    [Theory]
    [InlineData(nameof(Cfdi4Request.Fecha))]
    [InlineData(nameof(Cfdi4Request.LugarExpedicion))]
    [InlineData(nameof(Cfdi4Request.TipoComprobante))]
    [InlineData(nameof(Cfdi4Request.FormaPago))]
    [InlineData(nameof(Cfdi4Request.MetodoPago))]
    [InlineData(nameof(Cfdi4Request.Moneda))]
    [InlineData(nameof(Cfdi4Request.Subtotal))]
    [InlineData(nameof(Cfdi4Request.Total))]
    [InlineData(nameof(Cfdi4Request.Exportacion))]
    [InlineData(nameof(Receptor.DomicilioFiscalReceptor))]
    [InlineData(nameof(Receptor.RegimenFiscalReceptor))]
    public void Build_MissingRequiredNullableInput_ThrowsArgumentException(string propertyName)
    {
        var request = CreateRequest();
        typeof(Cfdi4Request).GetProperty(propertyName)?.SetValue(request, null);
        typeof(Receptor).GetProperty(propertyName)?.SetValue(request.Receptor, null);

        var exception = Assert.Throws<ArgumentException>(() => new CfdiXmlBuilder().Build(request));

        Assert.Contains(propertyName, exception.Message, StringComparison.Ordinal);
    }

    public static IEnumerable<object[]> InvalidCfdiRelacionados()
    {
        yield return new object[] { new CfdiRelacionados("", new List<string> { "12345678-1234-1234-1234-123456789012" }) };
        yield return new object[] { new CfdiRelacionados("01", new List<string>()) };
        yield return new object[] { new CfdiRelacionados("01", null!) };
        yield return new object[] { new CfdiRelacionados("01", new List<string> { "not-a-uuid" }) };
    }

    private static Cfdi4Request CreateRequest()
    {
        return new Cfdi4Request(
            new Emisor("KFR250210TQ1", "EMISOR", "601", "20000"),
            new Receptor("XAXX010101000", "PUBLICO EN GENERAL", "S01", "20000", "616"),
            new List<Concepto>
            {
                new("01010101", 1.5m, "H87", "Producto", 100m, 100m, "02")
            })
        {
            Fecha = new DateTime(2025, 2, 4, 13, 38, 31),
            LugarExpedicion = "20000",
            TipoComprobante = "I",
            FormaPago = "01",
            MetodoPago = "PUE",
            Moneda = "USD",
            TipoCambio = 1.25m,
            Subtotal = 100m,
            Total = 100m,
            Exportacion = "01"
        };
    }
}
