using ApiAggregator.Domain.Entities;
using ApiAggregator.Domain.Interfaces;

namespace ApiAggregator.Application.Queries.AggregatedData
{
    public class GetAggregatedDataQueryHandler : IRequestHandler<GetAggregatedDataQuery, IEnumerable<AggregatedResult>>
    {
        private readonly IEnumerable<IExternalApiService> _apiServices;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GetAggregatedDataQueryHandler> _logger;

        public GetAggregatedDataQueryHandler(
            IEnumerable<IExternalApiService> apiServices,
            IMemoryCache cache,
            ILogger<GetAggregatedDataQueryHandler> logger)
        {
            _apiServices = apiServices;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<AggregatedResult>> Handle(
            GetAggregatedDataQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = _apiServices
                .Where(api => request.Sources == null || request.Sources.Contains(api.ApiName))
                .Select(async api =>
                {
                    try
                    {
                        var cacheKey = $"api_data_{api.ApiName}";
                        if (_cache.TryGetValue(cacheKey, out AggregatedResult cachedResult))
                        {
                            return cachedResult;
                        }

                        var result = await api.FetchDataAsync(cancellationToken);
                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                        _cache.Set(cacheKey, result, cacheOptions);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error fetching data from {ApiName}", api.ApiName);
                        return null;
                    }
                });

            var results = (await Task.WhenAll(tasks))
                .Where(r => r != null);

            if (request.FromDate.HasValue)
            {
                results = results.Where(r => r.Timestamp >= request.FromDate.Value);
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                results = request.SortBy.ToLower() switch
                {
                    "date" => results.OrderBy(r => r.Timestamp),
                    "source" => results.OrderBy(r => r.Source),
                    "type" => results.OrderBy(r => r.Type),
                    _ => results
                };
            }

            return results;
        }
    }
}
