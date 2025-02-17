namespace ApiAggregation.Application.Dtos
{
    public class AggregatedDataDto
    {
        public List<WeatherDataDto> WeatherData { get; set; }
        public List<NewsDataDto> NewsData { get; set; }
        public List<GithubDataDto> GithubData { get; set; }
    }
}
