using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;
using TecnoFact.SDK.Http;
using TecnoFact.SDK.Exceptions;
using System.Net;
using System.Text.Json;
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

    [Fact]
    public async Task LoginAsync_WithUserCredentials_UsesPanelLoginAndBearerForSubsequentRequests()
    {
        var handler = new RecordingHttpMessageHandler(request =>
        {
            if (request.RequestUri!.AbsolutePath == "/api/login")
            {
                Assert.Equal("panelcfdi.tecnofact.mx", request.RequestUri.Host);
                Assert.Equal(HttpMethod.Post, request.Method);
                Assert.Null(request.Headers.Authorization);

                using var body = JsonDocument.Parse(request.Content!.ReadAsStringAsync().GetAwaiter().GetResult());
                Assert.Equal("user@example.com", body.RootElement.GetProperty("email").GetString());
                Assert.True(body.RootElement.GetProperty("password").GetString()!.Length > 0);

                return JsonResponse("{\"success\":true,\"access_token\":\"test-token\"}");
            }

            Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
            Assert.False(string.IsNullOrWhiteSpace(request.Headers.Authorization?.Parameter));
            return JsonResponse("{\"success\":true}");
        });
        var config = TecnoFactConfig.ForUserCredentials("user@example.com", "test-password", TecnoFactEnvironment.Production);
        using var client = new TecnoFactHttpClient(config, new HttpClient(handler));

        await client.LoginAsync();
        var response = await client.GetAsync<JsonElement>("/protected-resource");

        Assert.True(response.GetProperty("success").GetBoolean());
    }

    [Fact]
    public async Task LoginAsync_WithoutAccessToken_ThrowsAuthenticationException()
    {
        var handler = new RecordingHttpMessageHandler(_ => JsonResponse("{\"success\":true}"));
        var config = TecnoFactConfig.ForUserCredentials("user@example.com", "test-password", TecnoFactEnvironment.Production);
        using var client = new TecnoFactHttpClient(config, new HttpClient(handler));

        await Assert.ThrowsAsync<AuthenticationException>(() => client.LoginAsync());
    }

    [Fact]
    public async Task LoginAsync_WithInvalidAccessTokenFormat_ThrowsAuthenticationException()
    {
        var handler = new RecordingHttpMessageHandler(_ => JsonResponse("{\"access_token\":\"invalid\\u0001token\"}"));
        var config = TecnoFactConfig.ForUserCredentials("user@example.com", "test-password", TecnoFactEnvironment.Production);
        using var client = new TecnoFactHttpClient(config, new HttpClient(handler));

        await Assert.ThrowsAsync<AuthenticationException>(() => client.LoginAsync());
    }

    [Fact]
    public async Task LoginAsync_WithApiCredentials_RejectsLocallyWithoutSendingARequest()
    {
        var requestCount = 0;
        var handler = new RecordingHttpMessageHandler(_ =>
        {
            requestCount++;
            return JsonResponse("{\"success\":true}");
        });
        var config = new TecnoFactConfig("test-key", "test-secret", TecnoFactEnvironment.Production);
        using var client = new TecnoFactHttpClient(config, new HttpClient(handler));

        await Assert.ThrowsAsync<AuthenticationException>(() => client.LoginAsync());

        Assert.Equal(0, requestCount);
    }

    [Fact]
    public async Task Constructor_WithApiCredentials_KeepsBasicAuthorization()
    {
        var handler = new RecordingHttpMessageHandler(request =>
        {
            Assert.Equal("Basic", request.Headers.Authorization?.Scheme);
            Assert.False(string.IsNullOrWhiteSpace(request.Headers.Authorization?.Parameter));
            return JsonResponse("{\"success\":true}");
        });
        using var client = new TecnoFactHttpClient(_config, new HttpClient(handler));

        var response = await client.GetAsync<JsonElement>("/legacy-resource");

        Assert.True(response.GetProperty("success").GetBoolean());
    }

    private static HttpResponseMessage JsonResponse(string content) => new(HttpStatusCode.OK)
    {
        Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
    };

    private sealed class RecordingHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(handler(request));
    }
}
