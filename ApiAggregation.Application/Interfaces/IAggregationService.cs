using ApiAggregation.Application.Dtos;
using ApiAggregation.Domain.Entities;

namespace ApiAggregation.Application.Interfaces
{
    public interface IAggregationService
    {
        Task<AggregatedDataDto> GetAggregatedDataAsync();
    }
}