namespace ApiAggregation.Application.Dtos
{
    public class FilterRequestDto
    {
        public string? SortBy { get; set; }
        public string? FilterBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }
}
