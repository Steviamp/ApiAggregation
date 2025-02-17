using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ApiAggregation.Tests
{
    public class GetAggregatedDataQueryHandlerTests
    {
        private readonly Mock<IAggregationService> _mockAggregationService;
        private readonly GetAggregatedDataQueryHandler _handler;

        public GetAggregatedDataQueryHandlerTests()
        {
            _mockAggregationService = new Mock<IAggregationService>();
            _handler = new GetAggregatedDataQueryHandler(_mockAggregationService.Object);
        }

        [Fact]
        public async Task Handle_WithNoFilters_ReturnsAllData()
        {
            // Arrange
            var mockData = GetMockAggregatedData();
            _mockAggregationService.Setup(x => x.GetAggregatedDataAsync())
                .ReturnsAsync(mockData);

            var query = new GetAggregatedDataQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.WeatherData.Count);
            Assert.Equal(2, result.NewsData.Count);
            Assert.Equal(2, result.GithubData.Count);
        }

        [Fact]
        public async Task Handle_WithFilterBy_ReturnsFilteredData()
        {
            // Arrange
            var mockData = GetMockAggregatedData();
            _mockAggregationService.Setup(x => x.GetAggregatedDataAsync())
                .ReturnsAsync(mockData);

            var query = new GetAggregatedDataQuery { FilterBy = "London" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result.WeatherData);
            Assert.Equal("London", result.WeatherData[0].City);
        }

        [Fact]
        public async Task Handle_WithSortByDate_ReturnsSortedData()
        {
            // Arrange
            var mockData = GetMockAggregatedData();
            _mockAggregationService.Setup(x => x.GetAggregatedDataAsync())
                .ReturnsAsync(mockData);

            var query = new GetAggregatedDataQuery { SortBy = "date" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.WeatherData[0].Timestamp >= result.WeatherData[1].Timestamp);
            Assert.True(result.NewsData[0].PublishedAt >= result.NewsData[1].PublishedAt);
        }

        private static AggregatedData GetMockAggregatedData()
        {
            return new AggregatedData
            {
                WeatherData = new List<WeatherData>
                {
                    new WeatherData
                    {
                        City = "London",
                        Temperature = 20,
                        Condition = "Sunny",
                        Timestamp = DateTime.UtcNow
                    },
                    new WeatherData
                    {
                        City = "Paris",
                        Temperature = 25,
                        Condition = "Clear",
                        Timestamp = DateTime.UtcNow.AddHours(-1)
                    }
                },
                NewsData = new List<NewsData>
                {
                    new NewsData
                    {
                        Title = "Test News 1",
                        Description = "Description 1",
                        Source = "Source 1",
                        PublishedAt = DateTime.UtcNow
                    },
                    new NewsData
                    {
                        Title = "Test News 2",
                        Description = "Description 2",
                        Source = "Source 2",
                        PublishedAt = DateTime.UtcNow.AddHours(-2)
                    }
                },
                GithubData = new List<GithubData>
                {
                    new GithubData
                    {
                        RepositoryName = "Repo1",
                        Description = "Description 1",
                        Stars = 100,
                        LastUpdated = DateTime.UtcNow
                    },
                    new GithubData
                    {
                        RepositoryName = "Repo2",
                        Description = "Description 2",
                        Stars = 200,
                        LastUpdated = DateTime.UtcNow.AddHours(-3)
                    }
                }
            };
        }
    }
}
