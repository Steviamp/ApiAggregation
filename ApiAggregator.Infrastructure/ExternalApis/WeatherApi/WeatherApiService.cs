using System.Diagnostics;
using System.Net.Http.Json;

namespace ApiAggregator.Infrastructure.ExternalApis.WeatherApi
{
    public class WeatherApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiService> _logger;
        private readonly string _apiKey;
        private readonly IRequestStatisticsService _statisticsService;

        public string ApiName => "WeatherApi";

        public WeatherApiService(
            HttpClient httpClient,
            ILogger<WeatherApiService> logger,
            IConfiguration configuration,
            IRequestStatisticsService statisticsService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _statisticsService = statisticsService;
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
        }

        public async Task<AggregatedResult> FetchDataAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cities = new[] { "London", "New York", "Tokyo", "Paris", "Sydney" };
                var tasks = cities.Select(city => FetchCityWeatherAsync(city, cancellationToken));
                var weatherData = await Task.WhenAll(tasks);

                stopwatch.Stop();
                await _statisticsService.RecordRequestAsync(ApiName, stopwatch.ElapsedMilliseconds);

                return new AggregatedResult
                {
                    Source = ApiName,
                    Type = "Weather",
                    Data = new { Cities = weatherData },
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data");
                throw;
            }
        }

        private async Task<WeatherData> FetchCityWeatherAsync(string city, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(
                $"weather?q={city}&appid={_apiKey}&units=metric",
                cancellationToken);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WeatherData>(cancellationToken: cancellationToken);
        }
    }
}
