namespace ApiAggregator.Domain.Entities
{
    public class AggregatedResult
    {
        public string Source { get; set; }
        public string Type { get; set; }
        public object Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
