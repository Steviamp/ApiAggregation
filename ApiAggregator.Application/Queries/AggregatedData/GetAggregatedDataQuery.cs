using ApiAggregator.Domain.Entities;

namespace ApiAggregator.Application.Queries.AggregatedData
{
    public record GetAggregatedDataQuery(
    string[] Sources = null,
    DateTime? FromDate = null,
    string SortBy = null
    ) : IRequest<IEnumerable<AggregatedResult>>;
}
