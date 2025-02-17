using ApiAggregation.Application.Dtos;
using MediatR;

namespace ApiAggregation.Application.Queries.AggregatedData
{
    public class GetAggregatedDataQuery : IRequest<AggregatedDataDto>
    {
        public string? SortBy { get; set; }
        public string? FilterBy { get; set; }

        public GetAggregatedDataQuery(string? sortBy = null, string? filterBy = null)
        {
            SortBy = sortBy;
            FilterBy = filterBy;
        }
    }
}
