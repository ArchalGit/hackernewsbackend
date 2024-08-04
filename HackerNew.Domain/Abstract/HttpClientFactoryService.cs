using System;
using System.Net.Http;

public class HttpClientFactoryService : IHttpClientFactoryService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        return client;
    }
}
