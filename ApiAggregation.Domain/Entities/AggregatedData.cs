namespace ApiAggregation.Domain.Entities
{
    public class AggregatedData
    {
        public List<WeatherData> WeatherData { get; set; }
        public List<NewsData> NewsData { get; set; }
        public List<GithubData> GithubData { get; set; }
    }
}
