using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class ConfigTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesConfig()
    {
        var config = new TecnoFactConfig(
            apiKey: "test_key",
            apiSecret: "test_secret",
            environment: TecnoFactEnvironment.Sandbox,
            timeout: 30,
            retries: 3
        );

        Assert.Equal("test_key", config.ApiKey);
        Assert.Equal("test_secret", config.ApiSecret);
        Assert.Equal(TecnoFactEnvironment.Sandbox, config.Environment);
        Assert.Equal(30, config.Timeout);
        Assert.Equal(3, config.Retries);
    }

    [Fact]
    public void Constructor_WithEmptyApiKey_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new TecnoFactConfig("", "secret", TecnoFactEnvironment.Sandbox)
        );
    }

    [Fact]
    public void Constructor_WithEmptyApiSecret_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new TecnoFactConfig("key", "", TecnoFactEnvironment.Sandbox)
        );
    }

    [Fact]
    public void Constructor_WithNegativeTimeout_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new TecnoFactConfig("key", "secret", TecnoFactEnvironment.Sandbox, timeout: -1)
        );
    }

    [Fact]
    public void Constructor_WithNegativeRetries_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new TecnoFactConfig("key", "secret", TecnoFactEnvironment.Sandbox, retries: -1)
        );
    }

    [Fact]
    public void GetBaseUrl_WithSandbox_ReturnsSandboxUrl()
    {
        var config = new TecnoFactConfig("key", "secret", TecnoFactEnvironment.Sandbox);
        Assert.Equal("https://sandbox.tecnofact.com/api", config.GetBaseUrl());
    }

    [Fact]
    public void GetBaseUrl_WithProduction_ReturnsProductionUrl()
    {
        var config = new TecnoFactConfig("key", "secret", TecnoFactEnvironment.Production);
        Assert.Equal("https://api.tecnofact.com/api", config.GetBaseUrl());
    }

    [Fact]
    public void ToDictionary_ReturnsCorrectDictionary()
    {
        var config = new TecnoFactConfig("key", "secret", TecnoFactEnvironment.Sandbox, 30, 3);
        var dict = config.ToDictionary();

        Assert.Equal("key", dict["api_key"]);
        Assert.Equal("secret", dict["api_secret"]);
        Assert.Equal("sandbox", dict["environment"]);
        Assert.Equal(30, dict["timeout"]);
        Assert.Equal(3, dict["retries"]);
        Assert.Equal("https://sandbox.tecnofact.com/api", dict["base_url"]);
    }
}
