using ApiAggregator.Domain.Entities;

namespace ApiAggregator.Domain.Interfaces
{
    public interface IExternalApiService
    {
        string ApiName { get; }
        Task<AggregatedResult> FetchDataAsync(CancellationToken cancellationToken);
    }
}
