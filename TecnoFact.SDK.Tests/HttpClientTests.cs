using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;
using TecnoFact.SDK.Http;
using TecnoFact.SDK.Exceptions;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class HttpClientTests
{
    private readonly TecnoFactConfig _config;

    public HttpClientTests()
    {
        _config = new TecnoFactConfig(
            apiKey: "test_key",
            apiSecret: "test_secret",
            environment: TecnoFactEnvironment.Sandbox,
            timeout: 30,
            retries: 3
        );
    }

    [Fact]
    public void Constructor_WithValidConfig_CreatesHttpClient()
    {
        using var httpClient = new TecnoFactHttpClient(_config);
        
        Assert.NotNull(httpClient);
    }

    [Fact]
    public void Constructor_WithNullConfig_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new TecnoFactHttpClient(null!));
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var httpClient = new TecnoFactHttpClient(_config);
        
        httpClient.Dispose();
        httpClient.Dispose(); // Should not throw
        
        Assert.True(true); // If we get here, test passed
    }

    [Fact]
    public async Task GetAsync_WithInvalidUrl_ThrowsException()
    {
        using var httpClient = new TecnoFactHttpClient(_config);
        
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await httpClient.GetAsync<object>("/invalid-endpoint-that-does-not-exist-12345");
        });
    }

    [Fact]
    public async Task PostAsync_WithInvalidUrl_ThrowsException()
    {
        using var httpClient = new TecnoFactHttpClient(_config);
        var data = new { test = "data" };
        
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await httpClient.PostAsync<object, object>("/invalid-endpoint-that-does-not-exist-12345", data);
        });
    }

    [Fact]
    public void GetBaseUrl_ReturnsCorrectUrl()
    {
        using var httpClient = new TecnoFactHttpClient(_config);
        
        // El cliente debe usar la URL base de la configuración
        Assert.Equal("https://sandbox.tecnofact.com/api", _config.GetBaseUrl());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Config_WithDifferentRetries_IsValid(int retries)
    {
        var config = new TecnoFactConfig(
            apiKey: "test_key",
            apiSecret: "test_secret",
            environment: TecnoFactEnvironment.Sandbox,
            retries: retries
        );
        
        using var httpClient = new TecnoFactHttpClient(config);
        Assert.NotNull(httpClient);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(120)]
    public void Config_WithDifferentTimeouts_IsValid(int timeout)
    {
        var config = new TecnoFactConfig(
            apiKey: "test_key",
            apiSecret: "test_secret",
            environment: TecnoFactEnvironment.Sandbox,
            timeout: timeout
        );
        
        using var httpClient = new TecnoFactHttpClient(config);
        Assert.NotNull(httpClient);
    }
}
