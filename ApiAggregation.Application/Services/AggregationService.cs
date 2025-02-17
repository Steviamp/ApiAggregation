using ApiAggregation.Application.Interfaces;
using ApiAggregation.Domain.Entities;
using ApiAggregation.Domain.Interfaces;
using ApiAggregation.Infrastructure.HttpClients;

namespace ApiAggregation.Application.Services
{
    public class AggregationService : IAggregationService
    {
        private readonly WeatherApiClient _weatherClient;
        private readonly NewsApiClient _newsClient;
        private readonly GithubApiClient _githubClient;

        public AggregationService(
            WeatherApiClient weatherClient,
            NewsApiClient newsClient,
            GithubApiClient githubClient)
        {
            _weatherClient = weatherClient;
            _newsClient = newsClient;
            _githubClient = githubClient;
        }

        public async Task<AggregatedData> GetAggregatedDataAsync()
        {
            try
            {
                var weatherTask = _weatherClient.GetWeatherAsync("London");
                var newsTask = _newsClient.GetNewsAsync();
                var githubTask = _githubClient.GetTrendingRepositoriesAsync();

                await Task.WhenAll(weatherTask, newsTask, githubTask);

                var weather = weatherTask.Result;
                var news = newsTask.Result;
                var github = githubTask.Result;

                return new AggregatedData
                {
                    WeatherData = new List<WeatherData>
                    {
                        new WeatherData
                        {
                            City = weather.Name,
                            Temperature = weather.Main.Temp,
                            Condition = weather.Weather.FirstOrDefault()?.Description ?? "Unknown",
                            Timestamp = DateTime.UtcNow
                        }
                    },
                    NewsData = news.Articles.Select(a => new NewsData
                    {
                        Title = a.Title,
                        Description = a.Description,
                        Source = a.Source,
                        PublishedAt = a.PublishedAt
                    }).ToList(),
                    GithubData = github.Items.Select(r => new GithubData
                    {
                        RepositoryName = r.Name,
                        Description = r.Description,
                        Stars = r.StargazersCount,
                        LastUpdated = r.UpdatedAt
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to aggregate data", ex);
            }
        }
    }
}
