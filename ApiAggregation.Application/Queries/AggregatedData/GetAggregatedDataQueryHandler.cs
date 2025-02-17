using ApiAggregation.Application.Dtos;
using ApiAggregation.Application.Interfaces;
using MediatR;

namespace ApiAggregation.Application.Queries.AggregatedData
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
            var result = await _aggregationService.GetAggregatedDataAsync();

            // Apply filtering
            if (!string.IsNullOrEmpty(request.FilterBy))
            {
                result = FilterData(result, request.FilterBy);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                result = SortData(result, request.SortBy);
            }

            return result;
        }

        private AggregatedDataDto FilterData(AggregatedDataDto data, string filterBy)
        {
            filterBy = filterBy.ToLower();

            return new AggregatedDataDto
            {
                WeatherData = data.WeatherData
                    .Where(w =>
                        w.City.ToLower().Contains(filterBy) ||
                        w.Condition.ToLower().Contains(filterBy))
                    .ToList(),

                NewsData = data.NewsData
                    .Where(n =>
                        n.Title.ToLower().Contains(filterBy) ||
                        n.Description.ToLower().Contains(filterBy) ||
                        n.Source.ToLower().Contains(filterBy))
                    .ToList(),

                GithubData = data.GithubData
                    .Where(g =>
                        g.RepositoryName.ToLower().Contains(filterBy) ||
                        g.Description.ToLower().Contains(filterBy) ||
                        g.OwnerName.ToLower().Contains(filterBy))
                    .ToList()
            };
        }
        private AggregatedDataDto SortData(AggregatedDataDto data, string sortBy)
        {
            sortBy = sortBy.ToLower();

            return new AggregatedDataDto
            {
                WeatherData = sortBy switch
                {
                    "temperature" => data.WeatherData.OrderByDescending(w => w.Temperature).ToList(),
                    "city" => data.WeatherData.OrderBy(w => w.City).ToList(),
                    "date" => data.WeatherData.OrderByDescending(w => w.Timestamp).ToList(),
                    _ => data.WeatherData
                },

                // Sort News Data
                NewsData = sortBy switch
                {
                    "title" => data.NewsData.OrderBy(n => n.Title).ToList(),
                    "source" => data.NewsData.OrderBy(n => n.Source).ToList(),
                    "date" => data.NewsData.OrderByDescending(n => n.PublishedAt).ToList(),
                    _ => data.NewsData
                },

                // Sort Github Data
                GithubData = sortBy switch
                {
                    "name" => data.GithubData.OrderBy(g => g.RepositoryName).ToList(),
                    "stars" => data.GithubData.OrderByDescending(g => g.Stars).ToList(),
                    "date" => data.GithubData.OrderByDescending(g => g.LastUpdated).ToList(),
                    _ => data.GithubData
                }
            };
        }
    }
}
