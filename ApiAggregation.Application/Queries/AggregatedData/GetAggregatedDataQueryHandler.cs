using ApiAggregation.Application.Dtos;
using ApiAggregation.Application.Interfaces;
using ApiAggregation.Application.Queries.AggregatedData;
using MediatR;

namespace ApiAggregation.Application.Queries
{
    public class GetAggregatedDataQueryHandler : IRequestHandler<GetAggregatedDataQuery, AggregatedDataDto>
    {
        private readonly IAggregationService _aggregationService;

        public GetAggregatedDataQueryHandler(IAggregationService aggregationService)
        {
            _aggregationService = aggregationService;
        }

        public async Task<AggregatedDataDto> Handle(GetAggregatedDataQuery request, CancellationToken cancellationToken)
        {
            var aggregatedData = await _aggregationService.GetAggregatedDataAsync();

            // Apply filtering if requested
            if (!string.IsNullOrEmpty(request.FilterBy))
            {
                aggregatedData.WeatherData = aggregatedData.WeatherData
                    .Where(w => w.City.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase) ||
                               w.Condition.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                aggregatedData.NewsData = aggregatedData.NewsData
                    .Where(n => n.Title.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase) ||
                               n.Description.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase) ||
                               n.Source.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                aggregatedData.GithubData = aggregatedData.GithubData
                    .Where(g => g.RepositoryName.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase) ||
                               g.Description.Contains(request.FilterBy, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply sorting if requested
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "date":
                        aggregatedData.WeatherData = aggregatedData.WeatherData.OrderByDescending(w => w.Timestamp).ToList();
                        aggregatedData.NewsData = aggregatedData.NewsData.OrderByDescending(n => n.PublishedAt).ToList();
                        aggregatedData.GithubData = aggregatedData.GithubData.OrderByDescending(g => g.LastUpdated).ToList();
                        break;
                    case "name":
                        aggregatedData.WeatherData = aggregatedData.WeatherData.OrderBy(w => w.City).ToList();
                        aggregatedData.NewsData = aggregatedData.NewsData.OrderBy(n => n.Title).ToList();
                        aggregatedData.GithubData = aggregatedData.GithubData.OrderBy(g => g.RepositoryName).ToList();
                        break;
                }
            }

            return new AggregatedDataDto
            {
                WeatherData = aggregatedData.WeatherData.Select(w => new WeatherDataDto
                {
                    City = w.City,
                    Temperature = w.Temperature,
                    Condition = w.Condition,
                    Timestamp = w.Timestamp
                }).ToList(),

                NewsData = aggregatedData.NewsData.Select(n => new NewsDataDto
                {
                    Title = n.Title,
                    Description = n.Description,
                    Source = n.Source,
                    PublishedAt = n.PublishedAt
                }).ToList(),

                GithubData = aggregatedData.GithubData.Select(g => new GithubDataDto
                {
                    RepositoryName = g.RepositoryName,
                    Description = g.Description,
                    Stars = g.Stars,
                    LastUpdated = g.LastUpdated
                }).ToList()
            };
        }
    }
}