using System.Threading.Tasks;
using System;

namespace ApiAggregation.Tests.Services
{
    public class AggregationServiceTests
    {
        private readonly Mock<WeatherApiClient> _mockWeatherClient;
        private readonly Mock<NewsApiClient> _mockNewsClient;
        private readonly Mock<GithubApiClient> _mockGithubClient;
        private readonly AggregationService _service;

        public AggregationServiceTests()
        {
            _mockWeatherClient = new Mock<WeatherApiClient>();
            _mockNewsClient = new Mock<NewsApiClient>();
            _mockGithubClient = new Mock<GithubApiClient>();
            _service = new AggregationService(
                _mockWeatherClient.Object,
                _mockNewsClient.Object,
                _mockGithubClient.Object
            );
        }

        [Fact]
        public async Task GetAggregatedDataAsync_ReturnsAggregatedData()
        {
            // Arrange
            var weatherResponse = new WeatherApiResponse
            {
                Name = "London",
                Main = new WeatherApiResponse.MainData { Temp = 20 },
                Weather = new[] { new WeatherApiResponse.WeatherCondition { Description = "Sunny" } }
            };

            var newsResponse = new NewsApiResponse
            {
                Articles = new[]
                {
                    new NewsApiResponse.Article
                    {
                        Title = "Test News",
                        Description = "Test Description",
                        Source = "Test Source",
                        PublishedAt = DateTime.UtcNow
                    }
                }
            };

            var githubResponse = new GithubApiResponse
            {
                Items = new[]
                {
                    new GithubApiResponse.Repository
                    {
                        Name = "Test Repo",
                        Description = "Test Description",
                        StargazersCount = 100,
                        UpdatedAt = DateTime.UtcNow
                    }
                }
            };

            _mockWeatherClient.Setup(x => x.GetWeatherAsync(It.IsAny<string>()))
                .ReturnsAsync(weatherResponse);
            _mockNewsClient.Setup(x => x.GetNewsAsync())
                .ReturnsAsync(newsResponse);
            _mockGithubClient.Setup(x => x.GetTrendingRepositoriesAsync())
                .ReturnsAsync(githubResponse);

            // Act
            var result = await _service.GetAggregatedDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.WeatherData);
            Assert.Single(result.NewsData);
            Assert.Single(result.GithubData);
            Assert.Equal("London", result.WeatherData[0].City);
            Assert.Equal("Test News", result.NewsData[0].Title);
            Assert.Equal("Test Repo", result.GithubData[0].RepositoryName);
        }

        [Fact]
        public async Task GetAggregatedDataAsync_WhenApisFail_ThrowsException()
        {
            // Arrange
            _mockWeatherClient.Setup(x => x.GetWeatherAsync(It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException());

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(
                () => _service.GetAggregatedDataAsync()
            );
        }
    }
}
