namespace ApiAggregation.Domain.Entities
{
    public class GithubData
    {
        public string RepositoryName { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
