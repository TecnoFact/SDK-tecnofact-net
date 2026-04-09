using TecnoFact.SDK.Enums;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class EnumTests
{
    [Fact]
    public void TecnoFactEnvironment_IsProduction_ReturnsCorrectValue()
    {
        Assert.True(TecnoFactEnvironment.Production.IsProduction());
        Assert.False(TecnoFactEnvironment.Sandbox.IsProduction());
    }

    [Fact]
    public void TecnoFactEnvironment_IsSandbox_ReturnsCorrectValue()
    {
        Assert.True(TecnoFactEnvironment.Sandbox.IsSandbox());
        Assert.False(TecnoFactEnvironment.Production.IsSandbox());
    }

    [Fact]
    public void TecnoFactEnvironment_Label_ReturnsCorrectLabel()
    {
        Assert.Equal("Sandbox", TecnoFactEnvironment.Sandbox.Label());
        Assert.Equal("Producción", TecnoFactEnvironment.Production.Label());
    }

    [Fact]
    public void TecnoFactEnvironment_Value_ReturnsCorrectValue()
    {
        Assert.Equal("sandbox", TecnoFactEnvironment.Sandbox.Value());
        Assert.Equal("production", TecnoFactEnvironment.Production.Value());
    }

    [Fact]
    public void TecnoFactEnvironment_GetBaseUrl_ReturnsCorrectUrl()
    {
        Assert.Equal("https://sandbox.tecnofact.com/api", TecnoFactEnvironment.Sandbox.GetBaseUrl());
        Assert.Equal("https://api.tecnofact.com/api", TecnoFactEnvironment.Production.GetBaseUrl());
    }

    [Fact]
    public void TipoComprobante_GetCode_ReturnsCorrectCode()
    {
        Assert.Equal("I", TipoComprobante.Ingreso.GetCode());
        Assert.Equal("E", TipoComprobante.Egreso.GetCode());
        Assert.Equal("T", TipoComprobante.Traslado.GetCode());
        Assert.Equal("N", TipoComprobante.Nomina.GetCode());
        Assert.Equal("P", TipoComprobante.Pago.GetCode());
    }

    [Fact]
    public void TipoComprobante_GetDescription_ReturnsCorrectDescription()
    {
        Assert.Equal("Ingreso", TipoComprobante.Ingreso.GetDescription());
        Assert.Equal("Egreso", TipoComprobante.Egreso.GetDescription());
        Assert.Equal("Traslado", TipoComprobante.Traslado.GetDescription());
        Assert.Equal("Nómina", TipoComprobante.Nomina.GetDescription());
        Assert.Equal("Pago", TipoComprobante.Pago.GetDescription());
    }
}
