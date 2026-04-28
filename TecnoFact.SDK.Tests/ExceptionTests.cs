using TecnoFact.SDK.Exceptions;
using Xunit;

namespace TecnoFact.SDK.Tests;

public class ExceptionTests
{
    [Fact]
    public void TecnoFactException_WithMessage_CreatesException()
    {
        var exception = new TecnoFactException("Test error");
        
        Assert.Equal("Test error", exception.Message);
        Assert.Null(exception.Details);
        Assert.Null(exception.StatusCode);
    }

    [Fact]
    public void TecnoFactException_WithMessageAndDetails_CreatesException()
    {
        var details = new Dictionary<string, object> { ["field"] = "value" };
        var exception = new TecnoFactException("Test error", details);
        
        Assert.Equal("Test error", exception.Message);
        Assert.Equal(details, exception.Details);
        Assert.Null(exception.StatusCode);
    }

    [Fact]
    public void TecnoFactException_WithAllParameters_CreatesException()
    {
        var details = new Dictionary<string, object> { ["field"] = "value" };
        var exception = new TecnoFactException("Test error", details, 400);
        
        Assert.Equal("Test error", exception.Message);
        Assert.Equal(details, exception.Details);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public void AuthenticationException_InheritsFromTecnoFactException()
    {
        var exception = new AuthenticationException("Auth failed");
        
        Assert.IsType<AuthenticationException>(exception);
        Assert.IsAssignableFrom<TecnoFactException>(exception);
        Assert.Equal("Auth failed", exception.Message);
    }

    [Fact]
    public void ValidationException_WithDetails_CreatesException()
    {
        var details = new Dictionary<string, object> 
        { 
            ["rfc"] = "RFC inválido",
            ["total"] = "Total debe ser mayor a 0"
        };
        var exception = new ValidationException("Validation failed", details);
        
        Assert.Equal("Validation failed", exception.Message);
        Assert.Equal(details, exception.Details);
    }

    [Fact]
    public void TimbradoException_CreatesException()
    {
        var exception = new TimbradoException("Timbrado failed");
        
        Assert.IsType<TimbradoException>(exception);
        Assert.Equal("Timbrado failed", exception.Message);
    }

    [Fact]
    public void CancelacionException_CreatesException()
    {
        var exception = new CancelacionException("Cancelación failed");
        
        Assert.IsType<CancelacionException>(exception);
        Assert.Equal("Cancelación failed", exception.Message);
    }

    [Fact]
    public void NotFoundException_WithStatusCode_CreatesException()
    {
        var exception = new NotFoundException("Resource not found", null, 404);
        
        Assert.Equal("Resource not found", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public void RateLimitException_CreatesException()
    {
        var details = new Dictionary<string, object> 
        { 
            ["retry_after"] = 60,
            ["limit"] = 100
        };
        var exception = new RateLimitException("Rate limit exceeded", details, 429);
        
        Assert.Equal("Rate limit exceeded", exception.Message);
        Assert.Equal(429, exception.StatusCode);
        Assert.Equal(60, details["retry_after"]);
    }

    [Fact]
    public void ServerException_CreatesException()
    {
        var exception = new ServerException("Internal server error", null, 500);
        
        Assert.Equal("Internal server error", exception.Message);
        Assert.Equal(500, exception.StatusCode);
    }
}
