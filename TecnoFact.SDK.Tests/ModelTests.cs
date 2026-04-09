using TecnoFact.SDK.Models;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class ModelTests
{
    [Fact]
    public void Emisor_Constructor_SetsPropertiesCorrectly()
    {
        var emisor = new Emisor("RFC123", "Nombre Test", "601", "06300");

        Assert.Equal("RFC123", emisor.Rfc);
        Assert.Equal("Nombre Test", emisor.Nombre);
        Assert.Equal("601", emisor.RegimenFiscal);
        Assert.Equal("06300", emisor.Cp);
    }

    [Fact]
    public void Emisor_GetMethods_ReturnCorrectValues()
    {
        var emisor = new Emisor("RFC123", "Nombre Test", "601", "06300");

        Assert.Equal("RFC123", emisor.GetRfc());
        Assert.Equal("Nombre Test", emisor.GetNombre());
        Assert.Equal("601", emisor.GetRegimenFiscal());
        Assert.Equal("06300", emisor.GetCp());
    }

    [Fact]
    public void Emisor_ToDictionary_ReturnsCorrectDictionary()
    {
        var emisor = new Emisor("RFC123", "Nombre Test", "601", "06300");
        var dict = emisor.ToDictionary();

        Assert.Equal("RFC123", dict["rfc"]);
        Assert.Equal("Nombre Test", dict["nombre"]);
        Assert.Equal("601", dict["regimen_fiscal"]);
        Assert.Equal("06300", dict["cp"]);
    }

    [Fact]
    public void Receptor_Constructor_SetsPropertiesCorrectly()
    {
        var receptor = new Receptor("RFC456", "Cliente Test", "G03", "06300", "612");

        Assert.Equal("RFC456", receptor.Rfc);
        Assert.Equal("Cliente Test", receptor.Nombre);
        Assert.Equal("G03", receptor.UsoCfdi);
        Assert.Equal("06300", receptor.DomicilioFiscalReceptor);
        Assert.Equal("612", receptor.RegimenFiscalReceptor);
    }

    [Fact]
    public void Concepto_Constructor_SetsPropertiesCorrectly()
    {
        var concepto = new Concepto(
            claveProdServ: "01010101",
            cantidad: 1m,
            claveUnidad: "E48",
            descripcion: "Test",
            valorUnitario: 100m,
            importe: 100m
        );

        Assert.Equal("01010101", concepto.ClaveProdServ);
        Assert.Equal(1m, concepto.Cantidad);
        Assert.Equal("E48", concepto.ClaveUnidad);
        Assert.Equal("Test", concepto.Descripcion);
        Assert.Equal(100m, concepto.ValorUnitario);
        Assert.Equal(100m, concepto.Importe);
    }

    [Fact]
    public void Traslado_Constructor_SetsPropertiesCorrectly()
    {
        var traslado = new Traslado(
            baseImporte: 100m,
            impuesto: "002",
            tipoFactor: "Tasa",
            tasaOCuota: 0.16m,
            importe: 16m
        );

        Assert.Equal(100m, traslado.Base);
        Assert.Equal("002", traslado.Impuesto);
        Assert.Equal("Tasa", traslado.TipoFactor);
        Assert.Equal(0.16m, traslado.TasaOCuota);
        Assert.Equal(16m, traslado.Importe);
    }

    [Fact]
    public void Cfdi4Request_Constructor_SetsPropertiesCorrectly()
    {
        var emisor = new Emisor("RFC123", "Emisor", "601", "06300");
        var receptor = new Receptor("RFC456", "Receptor", "G03");
        var conceptos = new List<Concepto>
        {
            new Concepto("01010101", 1m, "E48", "Test", 100m, 100m)
        };

        var request = new Cfdi4Request(emisor, receptor, conceptos);

        Assert.Equal(emisor, request.Emisor);
        Assert.Equal(receptor, request.Receptor);
        Assert.Single(request.Conceptos);
    }
}
