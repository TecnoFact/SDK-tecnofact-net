using Moq;
using System.Text.Json;
using TecnoFact.SDK.Contracts;
using TecnoFact.SDK.Exceptions;
using TecnoFact.SDK.Models;
using TecnoFact.SDK.Services;

namespace TecnoFact.SDK.Tests;

public class CfdiServiceTests
{
    [Fact]
    public async Task TimbrarXmlAsync_PostsXmlToStampCfdiAndReturnsTypedResult()
    {
        const string xml = "<cfdi:Comprobante Version=\"4.0\" />";
        var expected = new ResultadoTimbrado
        {
            Success = true,
            Code = 200,
            XmlTimbrado = "<cfdi:Comprobante UUID=\"00000000-0000-0000-0000-000000000000\" />",
            Uuid = "00000000-0000-0000-0000-000000000000"
        };
        var httpClient = new Mock<IHttpClient>(MockBehavior.Strict);
        httpClient
            .Setup(client => client.PostAsync<TimbrarXmlRequest, ResultadoTimbrado>(
                "/api/v1/stamp-cfdi",
                It.Is<TimbrarXmlRequest>(request => request.Xml == xml),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var service = new CfdiService(httpClient.Object);

        var result = await service.TimbrarXmlAsync(new TimbrarXmlRequest(xml));

        Assert.Same(expected, result);
        httpClient.VerifyAll();
    }

    [Fact]
    public async Task TimbrarXmlAsync_WrapsTransportFailureInTimbradoException()
    {
        var httpClient = new Mock<IHttpClient>(MockBehavior.Strict);
        httpClient
            .Setup(client => client.PostAsync<TimbrarXmlRequest, ResultadoTimbrado>(
                "/api/v1/stamp-cfdi",
                It.IsAny<TimbrarXmlRequest>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("transport failure"));
        var service = new CfdiService(httpClient.Object);

        var exception = await Assert.ThrowsAsync<TimbradoException>(
            () => service.TimbrarXmlAsync(new TimbrarXmlRequest("<cfdi:Comprobante />")));

        Assert.Equal("Failed to stamp XML.", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void ResultadoTimbrado_MapsSupportedPhpResponseFields()
    {
        var result = JsonSerializer.Deserialize<ResultadoTimbrado>(
            """
            {
              "success": true,
              "code": "200",
              "error": "Panel response",
              "data": {
                "xml": "<cfdi:Comprobante />",
                "uuid": "00000000-0000-0000-0000-000000000000"
              }
            }
            """);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(200, result.Code);
        Assert.Equal("Panel response", result.Message);
        Assert.Equal("<cfdi:Comprobante />", result.XmlTimbrado);
        Assert.Equal("00000000-0000-0000-0000-000000000000", result.Uuid);
    }
}
