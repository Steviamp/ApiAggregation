using ApiAggregation.Application.Interfaces;
using ApiAggregation.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ApiAggregation.Application.Services
{
    public class WeatherApiService : IExternalApiService<WeatherData>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherData> FetchDataAsync()
        {
            try
            {
                var apiKey = _configuration["WeatherApi:ApiKey"];
                var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q=London&appid={apiKey}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return new WeatherData
                {
                    City = "London",
                    Temperature = 20,
                    Condition = "Sunny",
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to fetch weather data", ex);
            }
        }
    }
}
