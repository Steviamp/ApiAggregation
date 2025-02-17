using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ApiAggregation.Infrastructure.HttpClients
{
    public abstract class BaseApiClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly IConfiguration _configuration;

        protected BaseApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API request failed: {ex.Message}", ex);
            }
        }
    }
}
