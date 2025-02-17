namespace ApiAggregation.Domain.Entities
{
    public class WeatherData
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
