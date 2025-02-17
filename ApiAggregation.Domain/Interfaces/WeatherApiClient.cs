using ApiAggregation.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace ApiAggregation.Domain.Interfaces
{
    public class WeatherApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public string Name => "WeatherAPI";

        public WeatherApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:WeatherAPI"];
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
        }

        public async Task<T> FetchDataAsync<T>(IDictionary<string, string> parameters) where T : class
        {
            try
            {
                if (!parameters.TryGetValue("city", out var city))
                {
                    throw new ArgumentException("City parameter is required");
                }

                var response = await _httpClient.GetAsync(
                    $"weather?q={city}&appid={_apiKey}&units=metric");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            catch (HttpRequestException ex)
            {
                // Log the error
                throw new ExternalApiException($"Error fetching data from {Name}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ExternalApiException($"Unexpected error while fetching data from {Name}: {ex.Message}", ex);
            }
        }
    }
}
