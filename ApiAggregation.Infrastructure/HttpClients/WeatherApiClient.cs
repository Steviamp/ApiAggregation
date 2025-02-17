using ApiAggregation.Infrastructure.Models;
using Microsoft.Extensions.Configuration;

namespace ApiAggregation.Infrastructure.HttpClients
{
    public class WeatherApiClient : BaseApiClient
    {
        public WeatherApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
            var baseUrl = configuration["WeatherApi:BaseUrl"];
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<WeatherApiResponse> GetWeatherAsync(string city)
        {
            var apiKey = _configuration["WeatherApi:ApiKey"];
            var url = $"data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            return await GetAsync<WeatherApiResponse>(url);
        }
    }
}
