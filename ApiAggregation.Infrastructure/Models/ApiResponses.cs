namespace ApiAggregation.Infrastructure.Models
{
    public class WeatherApiResponse
    {
        public MainData Main { get; set; }
        public string Name { get; set; }
        public WeatherCondition[] Weather { get; set; }

        public class MainData
        {
            public double Temp { get; set; }
        }

        public class WeatherCondition
        {
            public string Description { get; set; }
        }
    }

    public class NewsApiResponse
    {
        public Article[]? Articles { get; set; }

        public class Article
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Source { get; set; }
            public DateTime PublishedAt { get; set; }
            public string? Url { get; set; }
        }
    }

    public class GithubApiResponse
    {
        public Repository[]? Items { get; set; }

        public class Repository
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public int StargazersCount { get; set; }
            public DateTime UpdatedAt { get; set; }
            public string? HtmlUrl { get; set; }
            public Owner Owner { get; set; }
        }

        public class Owner
        {
            public string? Login { get; set; }
        }
    }
}
