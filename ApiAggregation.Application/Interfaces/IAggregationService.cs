using ApiAggregation.Domain.Entities;

namespace ApiAggregation.Application.Interfaces
{
    public interface IAggregationService
    {
        Task<AggregatedData> GetAggregatedDataAsync();
    }
}