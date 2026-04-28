using TecnoFact.SDK.Models;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class ModelValidationTests
{
    [Fact]
    public void Cfdi4Request_WithValidData_CreatesRequest()
    {
        var emisor = new Emisor("XAXX010101000", "Emisor Test", "601", "06300");
        var receptor = new Receptor("XAXX010101001", "Receptor Test", "G03");
        var concepto = new Concepto("01010101", 1m, "E48", "Test", 100m, 100m);
        
        var request = new Cfdi4Request(emisor, receptor, new List<Concepto> { concepto });
        
        Assert.NotNull(request);
        Assert.Equal(emisor, request.Emisor);
        Assert.Equal(receptor, request.Receptor);
        Assert.Single(request.Conceptos);
    }

    [Fact]
    public void Cfdi4Request_WithMultipleConceptos_CreatesRequest()
    {
        var emisor = new Emisor("XAXX010101000", "Emisor Test", "601", "06300");
        var receptor = new Receptor("XAXX010101001", "Receptor Test", "G03");
        var conceptos = new List<Concepto>
        {
            new Concepto("01010101", 1m, "E48", "Concepto 1", 100m, 100m),
            new Concepto("01010102", 2m, "E48", "Concepto 2", 200m, 400m),
            new Concepto("01010103", 3m, "E48", "Concepto 3", 300m, 900m)
        };
        
        var request = new Cfdi4Request(emisor, receptor, conceptos);
        
        Assert.Equal(3, request.Conceptos.Count);
    }

    [Fact]
    public void Concepto_WithImpuestos_CreatesConcepto()
    {
        var traslado = new Traslado(100m, "002", "Tasa", 0.16m, 16m);
        var impuestos = new ImpuestosConcepto(
            traslados: new List<Traslado> { traslado }
        );
        
        var concepto = new Concepto(
            claveProdServ: "01010101",
            cantidad: 1m,
            claveUnidad: "E48",
            descripcion: "Test con impuestos",
            valorUnitario: 100m,
            importe: 100m,
            objetoImp: "02",
            impuestos: impuestos
        );
        
        Assert.NotNull(concepto.Impuestos);
        Assert.Single(concepto.Impuestos!.Traslados!);
    }

    [Fact]
    public void Impuestos_WithTrasladosAndRetenciones_CreatesImpuestos()
    {
        var traslados = new List<TrasladoGlobal>
        {
            new TrasladoGlobal("002", "Tasa", 0.16m, 160m)
        };
        
        var retenciones = new List<RetencionGlobal>
        {
            new RetencionGlobal("002", 10m)
        };
        
        var impuestos = new Impuestos(
            totalImpuestosTrasladados: 160m,
            totalImpuestosRetenidos: 10m,
            traslados: traslados,
            retenciones: retenciones
        );
        
        Assert.Equal(160m, impuestos.TotalImpuestosTrasladados);
        Assert.Equal(10m, impuestos.TotalImpuestosRetenidos);
        Assert.Single(impuestos.Traslados!);
        Assert.Single(impuestos.Retenciones!);
    }

    [Fact]
    public void CfdiRelacionados_WithUuids_CreatesCfdiRelacionados()
    {
        var uuids = new List<string>
        {
            "12345678-1234-1234-1234-123456789012",
            "87654321-4321-4321-4321-210987654321"
        };
        
        var relacionados = new CfdiRelacionados("01", uuids);
        
        Assert.Equal("01", relacionados.TipoRelacion);
        Assert.Equal(2, relacionados.Uuid.Count);
    }

    [Fact]
    public void Parte_WithValidData_CreatesParte()
    {
        var parte = new Parte(
            claveProdServ: "01010101",
            cantidad: 1m,
            descripcion: "Parte importada",
            valorUnitario: 100m,
            importe: 100m,
            noIdentificacion: "ABC123"
        );
        
        Assert.Equal("01010101", parte.ClaveProdServ);
        Assert.Equal(1m, parte.Cantidad);
        Assert.Equal("Parte importada", parte.Descripcion);
        Assert.Equal(100m, parte.ValorUnitario);
        Assert.Equal("ABC123", parte.NoIdentificacion);
    }

    [Fact]
    public void Concepto_ToDictionary_ReturnsValidDictionary()
    {
        var concepto = new Concepto("01010101", 1m, "E48", "Test", 100m, 100m);
        var dict = concepto.ToDictionary();
        
        Assert.Equal("01010101", dict["clave_prod_serv"]);
        Assert.Equal(1m, dict["cantidad"]);
        Assert.Equal("E48", dict["clave_unidad"]);
        Assert.Equal("Test", dict["descripcion"]);
        Assert.Equal(100m, dict["valor_unitario"]);
        Assert.Equal(100m, dict["importe"]);
    }

    [Fact]
    public void Cfdi4Request_ToDictionary_ReturnsValidDictionary()
    {
        var emisor = new Emisor("XAXX010101000", "Emisor", "601", "06300");
        var receptor = new Receptor("XAXX010101001", "Receptor", "G03");
        var concepto = new Concepto("01010101", 1m, "E48", "Test", 100m, 100m);
        
        var request = new Cfdi4Request(emisor, receptor, new List<Concepto> { concepto })
        {
            TipoComprobante = "I",
            FormaPago = "01",
            MetodoPago = "PUE",
            Moneda = "MXN",
            Subtotal = 100m,
            Total = 116m
        };
        
        var dict = request.ToDictionary();
        
        Assert.Equal("I", dict["tipo_comprobante"]);
        Assert.Equal("01", dict["forma_pago"]);
        Assert.Equal("PUE", dict["metodo_pago"]);
        Assert.Equal("MXN", dict["moneda"]);
        Assert.Equal(100m, dict["subtotal"]);
        Assert.Equal(116m, dict["total"]);
        Assert.Contains("emisor", dict.Keys);
        Assert.Contains("receptor", dict.Keys);
        Assert.Contains("conceptos", dict.Keys);
    }

    [Theory]
    [InlineData("XAXX010101000")]
    [InlineData("XEXX010101000")]
    [InlineData("CACX7605101P8")]
    public void Emisor_WithValidRfc_CreatesEmisor(string rfc)
    {
        var emisor = new Emisor(rfc, "Test", "601", "06300");
        
        Assert.Equal(rfc, emisor.Rfc);
    }

    [Theory]
    [InlineData("G01")]
    [InlineData("G02")]
    [InlineData("G03")]
    [InlineData("I01")]
    [InlineData("P01")]
    public void Receptor_WithValidUsoCfdi_CreatesReceptor(string usoCfdi)
    {
        var receptor = new Receptor("XAXX010101000", "Test", usoCfdi);
        
        Assert.Equal(usoCfdi, receptor.UsoCfdi);
    }
}
