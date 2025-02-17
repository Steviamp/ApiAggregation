using ApiAggregation.Application.Dtos;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Domain.Entities;

namespace ApiAggregation.Application.Services
{
    public class AggregationService : IAggregationService
    {
        private readonly IExternalApiService<WeatherData> _weatherService;
        private readonly IExternalApiService<NewsData> _newsService;
        private readonly IExternalApiService<GithubData> _githubService;

        public AggregationService(
            IExternalApiService<WeatherData> weatherService,
            IExternalApiService<NewsData> newsService,
            IExternalApiService<GithubData> githubService)
        {
            _weatherService = weatherService;
            _newsService = newsService;
            _githubService = githubService;
        }

        public async Task<AggregatedDataDto> GetAggregatedDataAsync()
        {
            try
            {
                var tasks = new Task[]
                {
                    _weatherService.FetchDataAsync(),
                    _newsService.FetchDataAsync(),
                    _githubService.FetchDataAsync()
                };

                await Task.WhenAll(tasks);

                return new AggregatedDataDto
                {
                    WeatherData = new List<WeatherData> { tasks[0].Result },
                    NewsData = new List<NewsData> { tasks[1].Result },
                    GithubData = new List<GithubData> { tasks[2].Result }
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to aggregate data", ex);
            }
        }
    }
}
