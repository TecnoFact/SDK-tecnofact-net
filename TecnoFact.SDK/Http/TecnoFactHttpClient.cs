using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TecnoFact.SDK.Config;
using TecnoFact.SDK.Contracts;
using TecnoFact.SDK.Exceptions;

namespace TecnoFact.SDK.Http;

/// <summary>
/// Cliente HTTP para las peticiones a la API de TecnoFact
/// </summary>
public class TecnoFactHttpClient : IHttpClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TecnoFactConfig _config;
    private readonly JsonSerializerOptions _jsonOptions;

    public TecnoFactHttpClient(TecnoFactConfig config, HttpClient? httpClient = null)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _httpClient = httpClient ?? new HttpClient();
        
        _httpClient.BaseAddress = new Uri(_config.GetBaseUrl());
        _httpClient.Timeout = TimeSpan.FromSeconds(_config.GetTimeout());
        
        var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_config.ApiKey}:{_config.ApiSecret}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "TecnoFact-SDK-NET/1.0.0");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<TResponse> GetAsync<TResponse>(string endpoint, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, queryParams);
        return await ExecuteWithRetryAsync<TResponse>(
            async () => await _httpClient.GetAsync(url, cancellationToken),
            cancellationToken
        );
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetryAsync<TResponse>(
            async () =>
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await _httpClient.PostAsync(endpoint, content, cancellationToken);
            },
            cancellationToken
        );
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetryAsync<TResponse>(
            async () =>
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await _httpClient.PutAsync(endpoint, content, cancellationToken);
            },
            cancellationToken
        );
    }

    public async Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetryAsync<TResponse>(
            async () => await _httpClient.DeleteAsync(endpoint, cancellationToken),
            cancellationToken
        );
    }

    public async Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetryAsync<TResponse>(
            async () =>
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Patch, endpoint) { Content = content };
                return await _httpClient.SendAsync(request, cancellationToken);
            },
            cancellationToken
        );
    }

    private async Task<TResponse> ExecuteWithRetryAsync<TResponse>(
        Func<Task<HttpResponseMessage>> requestFunc,
        CancellationToken cancellationToken)
    {
        Exception? lastException = null;
        
        for (int attempt = 0; attempt <= _config.GetRetries(); attempt++)
        {
            try
            {
                var response = await requestFunc();
                return await HandleResponseAsync<TResponse>(response);
            }
            catch (Exception ex) when (attempt < _config.GetRetries() && IsRetryableException(ex))
            {
                lastException = ex;
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
            }
        }

        throw lastException ?? new TecnoFactException("Request failed after retries");
    }

    private async Task<TResponse> HandleResponseAsync<TResponse>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions)
                ?? throw new TecnoFactException("Failed to deserialize response");
        }

        var statusCode = (int)response.StatusCode;
        var errorDetails = TryParseErrorDetails(content);

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthenticationException("Authentication failed", errorDetails, statusCode),
            HttpStatusCode.BadRequest => new ValidationException("Validation error", errorDetails, statusCode),
            HttpStatusCode.NotFound => new NotFoundException("Resource not found", errorDetails, statusCode),
            HttpStatusCode.TooManyRequests => new RateLimitException("Rate limit exceeded", errorDetails, statusCode),
            HttpStatusCode.InternalServerError or HttpStatusCode.BadGateway or HttpStatusCode.ServiceUnavailable 
                => new ServerException("Server error", errorDetails, statusCode),
            _ => new TecnoFactException($"Request failed with status {statusCode}", errorDetails, statusCode)
        };
    }

    private Dictionary<string, object>? TryParseErrorDetails(string content)
    {
        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(content, _jsonOptions);
        }
        catch
        {
            return new Dictionary<string, object> { ["raw_error"] = content };
        }
    }

    private bool IsRetryableException(Exception ex)
    {
        return ex is HttpRequestException or TaskCanceledException;
    }

    private string BuildUrl(string endpoint, Dictionary<string, string>? queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return endpoint;

        var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{endpoint}?{query}";
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
