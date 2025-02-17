using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregator.Infrastructure.ExternalApis.NewsApi
{
    public class NewsApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NewsApiService> _logger;
        private readonly string _apiKey;
        private readonly IRequestStatisticsService _statisticsService;

        public string ApiName => "NewsApi";

        public NewsApiService(
            HttpClient httpClient,
            ILogger<NewsApiService> logger,
            IConfiguration configuration,
            IRequestStatisticsService statisticsService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["NewsApi:ApiKey"];
            _statisticsService = statisticsService;
            _httpClient.BaseAddress = new Uri("https://newsapi.org/v2/");
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
        }

        public async Task<AggregatedResult> FetchDataAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var categories = new[] { "technology", "business", "science" };
                var tasks = categories.Select(category => FetchNewsByCategoryAsync(category, cancellationToken));
                var newsData = await Task.WhenAll(tasks);

                stopwatch.Stop();
                await _statisticsService.RecordRequestAsync(ApiName, stopwatch.ElapsedMilliseconds);

                return new AggregatedResult
                {
                    Source = ApiName,
                    Type = "News",
                    Data = new { Categories = newsData },
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news data");
                throw;
            }
        }

        private async Task<NewsCategory> FetchNewsByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(
                $"top-headlines?category={category}&language=en&pageSize=5",
                cancellationToken);

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<NewsApiResponse>(cancellationToken: cancellationToken);

            return new NewsCategory
            {
                Category = category,
                Articles = data.Articles
            };
        }
    }
}
